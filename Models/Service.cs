﻿namespace WebApplicationAnnuaire.Models
{
    public class Service
    {
        public int? Id { get; set; }
        public string? Nom { get; set; }

        public ICollection<Employe>? Employes { get; set; }

    }
}
