using Bogus;
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

        public async Task SeedDataAsync()
        {
            if (await Employes.AnyAsync()) // Vérifie si des données existent déjà dans la base de données
            {
                return; // Si des données existent, ne rien faire
            }

            var bogus = new Faker<Employe>()
                .RuleFor(e => e.Nom, f => f.Name.LastName())
                .RuleFor(e => e.Prenom, f => f.Name.FirstName())
                .RuleFor(e => e.TelephoneFixe, f => f.Phone.PhoneNumber())
                .RuleFor(e => e.TelephonePortable, f => f.Phone.PhoneNumber())
                .RuleFor(e => e.Email, f => f.Internet.Email())
                .RuleFor(e => e.ServiceId, f => f.Random.Number(1, 5))
                .RuleFor(e => e.SiteId, f => f.Random.Number(1, 2));

            var employes = bogus.Generate(1000);

            await Employes.AddRangeAsync(employes);
            await SaveChangesAsync();
        }
    }
}