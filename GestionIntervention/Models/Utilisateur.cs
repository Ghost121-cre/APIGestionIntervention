// Models/Utilisateur.cs
using System;
using System.Collections.Generic;

namespace GestionIntervention.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Prenom { get; set; } = null!;
        public string Nom { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MotDePasseHash { get; set; } = null!;
        public string? Telephone { get; set; }
        public string? Role { get; set; }
        public string? Bio { get; set; }
        public string? Pays { get; set; }
        public string? Ville {  get; set; }
        public string? Avatar { get; set; }
        public string? Statut { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;
        public bool PremiereConnexion { get; set; } = true; 

        public ICollection<Intervention>? Interventions { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<StatutIntervention>? StatutsInterventions { get; set; }
    }
}