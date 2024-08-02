using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApplication1.Data;
using WebApplication1.Helper;
using WebApplication1.Helper.Service;
using zadatak1.Data.Models;

namespace zadatak1.Controllers
{
    [ApiController]
    [Route("users")]
    [AutorizacijaAtribut]
    public class KorisnickiNalogControlller : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        public KorisnickiNalogControlller(ApplicationDbContext dbContext, MyAuthService authService)
        {
            this._dbContext = dbContext;
            _authService = authService;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KorisnickiNalog>>> GetUsers()
        {
            

            return await _dbContext.KorisnickiNalog.ToListAsync();
        }
        // GET: api/users/details/{id}
        [HttpGet("details/{id}")]
        public async Task<ActionResult<KorisnickiNalog>> GetUserDetails(int id)
        {
            var korisnik = await _dbContext.KorisnickiNalog.FindAsync(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return korisnik;
        }
        // POST: api/users/block/{id}
        [HttpPost("block/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var korisnik = await _dbContext.KorisnickiNalog.FindAsync(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            // Logic for blocking user
            korisnik.isBlokiran = true;
            _dbContext.Entry(korisnik).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        public class RegistracijaVM
        {
            public string Ime { get; set; }
            public string KorisnickoIme { get; set; }
            public string Lozinka { get; set; }
            public string Slika { get; set; } = null!;
            public DateTime DatumRodjenja { get; set; }
            public int BrojNarudzbi { get; set; }

        }
        // POST: api/users/register
        [HttpPost("register")]
        public  ActionResult RegisterUser([FromBody] RegistracijaVM registracijaVM)
        {
            var noviKorisnik = new KorisnickiNalog()
            {
                Ime = registracijaVM.Ime,
                KorisnickoIme = registracijaVM.KorisnickoIme,
                Lozinka = registracijaVM.Lozinka,
                Slika = registracijaVM.Slika,               
                DatumRodjenja = registracijaVM.DatumRodjenja,
                BrojNarudzbi = registracijaVM.BrojNarudzbi,
                Status = true, 
                isBlokiran = false, 
                isUposlenik = true, 
                isAdministrator = false
            };
            _dbContext.KorisnickiNalog.Add(noviKorisnik);
            _dbContext.SaveChanges();
            return Ok(noviKorisnik.Id);
        }
        public class AzuriranjeKorisnikaVM
        {
            public string Ime { get; set; }
            public string KorisnickoIme { get; set; }
            public string Slika { get; set; }
            public DateTime DatumRodjenja { get; set; }
            public bool Status { get; set; }
            public bool isBlokiran { get; set; }
            public int BrojNarudzbi { get; set; }
        }
        
        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] AzuriranjeKorisnikaVM azuriranjeVM)
        {
            var korisnik = await _dbContext.KorisnickiNalog.FindAsync(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            korisnik.Ime = azuriranjeVM.Ime;
            korisnik.KorisnickoIme = azuriranjeVM.KorisnickoIme;
            korisnik.Slika = azuriranjeVM.Slika;       
            korisnik.DatumRodjenja = azuriranjeVM.DatumRodjenja;
            korisnik.Status = azuriranjeVM.Status;
            korisnik.isBlokiran = azuriranjeVM.isBlokiran;
            korisnik.BrojNarudzbi = azuriranjeVM.BrojNarudzbi;

            _dbContext.Entry(korisnik).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }


    }

}
