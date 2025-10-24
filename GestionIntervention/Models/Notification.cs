using System;

namespace GestionIntervention.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UtilisateurId { get; set; }
        public string Titre { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Type { get; set; } = null!;
        public bool Lu { get; set; } = false;
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public Utilisateur? Utilisateur { get; set; }
    }
}
