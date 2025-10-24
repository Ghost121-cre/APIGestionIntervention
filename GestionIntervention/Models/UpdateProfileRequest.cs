using System.ComponentModel.DataAnnotations;

public class UpdateProfileRequest
{
    [Required]
    public string Prenom { get; set; } = null!;

    [Required]
    public string Nom { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public string? Telephone { get; set; }
    public string? Bio { get; set; }
    public string? Pays { get; set; }
    public string? Ville { get; set; }
    public string? Avatar { get; set; }
}