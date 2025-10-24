using System;
using System.Collections.Generic;

namespace GestionIntervention.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Nom { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Telephone { get; set; }
        public string? Adresse { get; set; }
        public string? Ville { get; set; }
        public string? CodePostal { get; set; }
        public string? Pays { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public ICollection<Incident>? Incidents { get; set; }
        public ICollection<Intervention>? Interventions { get; set; }
    }
}
