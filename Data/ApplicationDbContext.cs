using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplicationAnnuaire.Models;

namespace WebApplicationAnnuaire.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employe> Employes { get; set; }

        public DbSet<WebApplicationAnnuaire.Models.Service> Service { get; set; } = default!;

        public DbSet<WebApplicationAnnuaire.Models.Site> Site { get; set; } = default!;

        public DbSet<WebApplicationAnnuaire.Models.Utilisateur> Utilisateur { get; set; } = default!;
    }
}