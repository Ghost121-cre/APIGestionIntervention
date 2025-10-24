// Controllers/AuthController.cs
using GestionIntervention.Data;
using GestionIntervention.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            Console.WriteLine($"🔐 Tentative de connexion pour: {loginRequest.Email}");

            // Chercher l'utilisateur par email
            var user = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequest.Email.Trim().ToLower());

            if (user == null)
            {
                Console.WriteLine("❌ Utilisateur non trouvé");
                return Unauthorized(new { message = "Email ou mot de passe incorrect" });
            }

            Console.WriteLine($"✅ Utilisateur trouvé: {user.Prenom} {user.Nom}");

            // Vérifier le mot de passe (comparaison directe)
            if (loginRequest.MotDePasseHash != user.MotDePasseHash)
            {
                Console.WriteLine("❌ Mot de passe incorrect");
                return Unauthorized(new { message = "Email ou mot de passe incorrect" });
            }

            Console.WriteLine("🎉 Connexion réussie!");

            // Connexion réussie !
            return Ok(new
            {
                id = user.Id,
                prenom = user.Prenom,
                nom = user.Nom,
                email = user.Email,
                telephone = user.Telephone,
                role = user.Role,
                statut = user.Statut,
                dateCreation = user.DateCreation,
                premiereConnexion = user.PremiereConnexion,
                bio = user.Bio,
                pays = user.Pays,
                ville = user.Ville,
                avatar = user.Avatar,
                token = "token-temporaire"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Erreur serveur: {ex.Message}");
            return StatusCode(500, new { message = "Erreur interne du serveur" });
        }
    }

    // 🔥 AJOUTEZ CETTE MÉTHODE POUR LE CHANGEMENT DE MOT DE PASSE
    [HttpPost("change-password-first-login")]
    public async Task<ActionResult> ChangePasswordFirstLogin([FromBody] ChangePasswordFirstLoginRequest request)
    {
        try
        {
            Console.WriteLine($"🔑 Changement mot de passe première connexion pour: {request.UserId}");

            // Trouver l'utilisateur
            var user = await _context.Utilisateurs.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new { message = "Utilisateur non trouvé" });
            }

            Console.WriteLine($"✅ Utilisateur trouvé: {user.Prenom} {user.Nom}");
            Console.WriteLine($"🔄 Ancien mot de passe: {user.MotDePasseHash}");
            Console.WriteLine($"🆕 Nouveau mot de passe: {request.NewPassword}");

            // Mettre à jour le mot de passe
            user.MotDePasseHash = request.NewPassword;
            user.PremiereConnexion = false; // Marquer que la première connexion est terminée

            // Sauvegarder en base
            await _context.SaveChangesAsync();

            Console.WriteLine("🎉 Mot de passe changé avec succès!");

            return Ok(new
            {
                message = "Mot de passe modifié avec succès",
                premiereConnexion = false
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Erreur: {ex.Message}");
            return StatusCode(500, new { message = "Erreur lors du changement de mot de passe" });
        }
    }

    
    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            Console.WriteLine($"🔑 Changement mot de passe pour: {request.UserId}");

            var user = await _context.Utilisateurs.FindAsync(request.UserId);
            if (user == null)
                return NotFound(new { message = "Utilisateur non trouvé" });

            // Vérifier l'ancien mot de passe
            if (request.CurrentPassword != user.MotDePasseHash)
            {
                return Unauthorized(new { message = "Mot de passe actuel incorrect" });
            }

            // Mettre à jour le mot de passe
            user.MotDePasseHash = request.NewPassword;

            await _context.SaveChangesAsync();

            Console.WriteLine("✅ Mot de passe changé avec succès");
            return Ok(new { message = "Mot de passe modifié avec succès" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Erreur changement mot de passe: {ex.Message}");
            return StatusCode(500, new { message = "Erreur lors du changement de mot de passe" });
        }
    }


}
