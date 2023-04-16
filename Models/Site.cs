namespace WebApplicationAnnuaire.Models
{
    public class Site
    {
        public int? Id { get; set; }
        public string? Ville { get; set; }

        public ICollection<Employe>? Employes { get; set; }
    }
}
