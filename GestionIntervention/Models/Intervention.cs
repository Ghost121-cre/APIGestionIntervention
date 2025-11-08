using System;
using System.Collections.Generic;

namespace GestionIntervention.Models
{
    public class Intervention
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ProduitId { get; set; }
        public int? IncidentId { get; set; }
        public string Description { get; set; } = null!;
        public string Priorite { get; set; } = null!;
        public string Statut { get; set; } = null!;
        public DateTime DatePlanifiee { get; set; }
        public int TechnicienId { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }

        public Client? Client { get; set; }
        public Produit? Produit { get; set; }
        public Incident? Incident { get; set; }
        public Utilisateur? Technicien { get; set; }

        public ICollection<InterventionImage>? Images { get; set; }
        public ICollection<Rapport>? Rapports { get; set; }
    }
}