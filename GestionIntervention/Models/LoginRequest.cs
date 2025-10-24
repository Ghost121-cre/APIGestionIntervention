// Models/LoginRequest.cs
using System.ComponentModel.DataAnnotations;

namespace GestionIntervention.Models
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string MotDePasseHash { get; set; } = null!;
    }
}