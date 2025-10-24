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
        public required string NomFichier { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Chemin { get; set; }

        public DateTime DateUpload { get; set; } = DateTime.Now;

        // 🔗 Relation
        public Intervention? Intervention { get; set; }
    }
}
