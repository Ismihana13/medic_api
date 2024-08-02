using Microsoft.EntityFrameworkCore;
using zadatak1.Data.Models;



namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<KorisnickiNalog> KorisnickiNalog { get; set; }
        public DbSet<AutentifikacijaToken> AutentifikacijaToken { get; set; }
        
        public ApplicationDbContext(
          DbContextOptions options) : base(options)
        {
        }

    }
}
