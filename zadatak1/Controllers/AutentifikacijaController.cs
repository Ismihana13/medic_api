
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using static WebApplication1.Helper.MyAuthTokenExtension;
using zadatak1.Data.Models;
using WebApplication1.Modul_Autentifikacija.ViewModels;
using WebApplication1.Helper;

namespace FIT_Api_Examples.Modul0_Autentifikacija.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AutentifikacijaController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
       

        public AutentifikacijaController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
            
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginInformacije>> Login([FromBody] LoginVM x, CancellationToken cancellationToken)
        {
            //1- provjera logina
            var logiraniKorisnik = await _dbContext.KorisnickiNalog
         .FirstOrDefaultAsync(k =>
             k.KorisnickoIme == x.korisnickoIme &&
             k.Lozinka == x.lozinka,
             cancellationToken);

            if (logiraniKorisnik == null)
            {
                //pogresan username i password
                return new LoginInformacije(null);
            }
         

            //2- generisati random string
            string randomString = TokenGenerator.Generate(10);

            //3- dodati novi zapis u tabelu AutentifikacijaToken za logiraniKorisnikId i randomString
            var noviToken = new AutentifikacijaToken()
            {
                IpAdresa = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                Vrijednost = randomString,
                KorisnickiNalog = logiraniKorisnik,
                VrijemeEvidentiranja = DateTime.Now,
                
            };

             _dbContext.Add(noviToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            
            //4- vratiti token string
            return new LoginInformacije(noviToken);
        }
      
        [HttpPost("logout")]
        public async Task<NoResponse> Logout(CancellationToken cancellationToken)
        {
            AutentifikacijaToken autentifikacijaToken = HttpContext.GetAuthToken();

            if (autentifikacijaToken == null)
                return new NoResponse();

            _dbContext.Remove(autentifikacijaToken);
           await _dbContext.SaveChangesAsync(cancellationToken);
            return new NoResponse();

        }

        [HttpGet]
        public ActionResult<AutentifikacijaToken> Get()
        {
            AutentifikacijaToken autentifikacijaToken = HttpContext.GetAuthToken();

            return autentifikacijaToken;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DateTime?>> GetUserTime(int id)
        {
            var autentifikacijaToken = await _dbContext.AutentifikacijaToken
                .Where(t => t.KorisnickiNalogId == id)
                .OrderByDescending(t => t.VrijemeEvidentiranja)
                .FirstOrDefaultAsync();

            if (autentifikacijaToken == null)
            {
                return Ok((DateTime?)null);  // Vraća null umesto 404
            }

            return Ok(autentifikacijaToken.VrijemeEvidentiranja);
        }



    }
}