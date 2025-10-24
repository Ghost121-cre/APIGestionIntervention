using System.ComponentModel.DataAnnotations;

namespace GestionIntervention.Models
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "L'ID utilisateur est requis")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Le mot de passe actuel est requis")]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "Le nouveau mot de passe est requis")]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string NewPassword { get; set; } = null!;
    }
}