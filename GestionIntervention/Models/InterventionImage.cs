using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionIntervention.Models
{
    public class InterventionImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Intervention")]
        public int InterventionId { get; set; }

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
        public Intervention? Intervention { get; set; }
    }
}