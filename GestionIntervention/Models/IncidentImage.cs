using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionIntervention.Models
{
    public class IncidentImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Incident")]
        public int IncidentId { get; set; }

        [Required]
        [MaxLength(255)]
        public string NomFichier { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Chemin { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? AltText { get; set; }

        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

        // Relation
        public Incident? Incident { get; set; }
    }
}