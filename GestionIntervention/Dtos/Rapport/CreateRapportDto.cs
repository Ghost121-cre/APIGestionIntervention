using System.ComponentModel.DataAnnotations;

namespace GestionIntervention.DTOs.Rapport
{
    public class CreateRapportDto
    {
        [Required(ErrorMessage = "L'ID de l'intervention est requis")]
        public int InterventionId { get; set; }

        [Required(ErrorMessage = "La date du rapport est requise")]
        public DateTime DateRapport { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Le client est requis")]
        public string Client { get; set; } = null!;

        public List<string> Intervenant { get; set; } = new List<string>();

        [Required(ErrorMessage = "Le type d'intervention est requis")]
        public string TypeIntervention { get; set; } = null!;

        [Required(ErrorMessage = "La description est requise")]
        public string Description { get; set; } = null!;

        public string? Observations { get; set; }
        public string? TravauxEffectues { get; set; }
        public string? HeureDebut { get; set; }
        public string? HeureFin { get; set; }
    }
}