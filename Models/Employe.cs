using System.ComponentModel;

namespace WebApplicationAnnuaire.Models
{
    public class Employe
    {
        public int? Id { get; set; }

        [DisplayName("Nom")]
        public string? Nom { get; set; }

        [DisplayName("Prénom")]
        public string? Prenom { get; set; }

        [DisplayName("Téléphone Fixe")]
        public string? TelephoneFixe { get; set; }

        [DisplayName("Téléphone Portable")]
        public string? TelephonePortable { get; set; }

        [DisplayName("E-mail")]
        public string? Email { get; set; }

        [DisplayName("Service")]
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }

        [DisplayName("Site")]
        public int? SiteId { get; set; }
        public Site? Site { get; set; }

    }
}
