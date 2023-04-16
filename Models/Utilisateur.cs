namespace WebApplicationAnnuaire.Models
{
    public class Utilisateur
    {
        public int? Id { get; set; }
        public string? NomUtilisateur { get; set; }
        public string? MotDePasse { get; set; }
        public bool? EstAdmin { get; set; }
    }
}
