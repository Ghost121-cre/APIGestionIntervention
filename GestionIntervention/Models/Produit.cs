using System;
using System.Collections.Generic;

namespace GestionIntervention.Models
{
    public class Produit
    {
        public int Id { get; set; }
        public string Nom { get; set; } = null!;
        public string? Description { get; set; }
        public string Categorie { get; set; } = null!;
        public string Statut { get; set; } = "Actif";
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public ICollection<Incident>? Incidents { get; set; }
        public ICollection<Intervention>? Interventions { get; set; }
    }
}
