using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace zadatak1.Data.Models
{
    public class KorisnickiNalog
    {
        [Key]
        public int Id { get; set; }
        public string Ime { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Slika { get; set; } = null!;

        public DateTime DatumRodjenja { get; set; }
        public bool Status { get; set; } =true;
        public bool isBlokiran { get; set; } = false;

        public bool isUposlenik { get; set; }
        public bool isAdministrator { get; set; }
        public int BrojNarudzbi { get; set; }

    }
}
