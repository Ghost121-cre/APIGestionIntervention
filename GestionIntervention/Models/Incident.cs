using System;
using System.Collections.Generic;

namespace GestionIntervention.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ProduitId { get; set; }
        public string Description { get; set; } = null!;
        public string Priorite { get; set; } = null!;
        public string Statut { get; set; } = null!;
        public DateTime DateSurvenu { get; set; }
        public DateTime? DateResolution { get; set; }

        public Client? Client { get; set; }
        public Produit? Produit { get; set; }
        public ICollection<IncidentImage>? Images { get; set; }
        public ICollection<Intervention>? Interventions { get; set; }
    }
}
