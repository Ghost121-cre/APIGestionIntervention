using System;

namespace GestionIntervention.Models
{
    public class StatutIntervention
    {
        public int Id { get; set; }
        public int InterventionId { get; set; }
        public string Statut { get; set; } = null!;
        public DateTime DateChangement { get; set; } = DateTime.Now;
        public int UtilisateurId { get; set; }
        public string? Commentaire { get; set; }

        public Intervention? Intervention { get; set; }
        public Utilisateur? Utilisateur { get; set; }
    }
}
