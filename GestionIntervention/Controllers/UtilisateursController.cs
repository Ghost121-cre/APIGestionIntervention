using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionIntervention.Data;
using GestionIntervention.Models;

namespace GestionIntervention.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UtilisateursController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            return await _context.Utilisateurs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Utilisateur>> GetUtilisateur(int id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
                return NotFound();

            return utilisateur;
        }

        [HttpPost]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            
            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUtilisateur), new { id = utilisateur.Id }, utilisateur);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.Id) return BadRequest();

            _context.Entry(utilisateur).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("update-profile/{id}")]
        public async Task<ActionResult<Utilisateur>> UpdateProfile(int id, [FromBody] UpdateProfileRequest request)
        {
            try
            {
                var user = await _context.Utilisateurs.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = "Utilisateur non trouvé" });

                // Mettre à jour les champs
                user.Prenom = request.Prenom;
                user.Nom = request.Nom;
                user.Email = request.Email;
                user.Telephone = request.Telephone;
                user.Bio = request.Bio;
                user.Pays = request.Pays;
                user.Ville = request.Ville;
                user.Avatar = request.Avatar;

                await _context.SaveChangesAsync();

                // 🔥 RETOURNER L'UTILISATEUR COMPLET
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
                    avatar = user.Avatar
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erreur lors de la mise à jour: {ex.Message}" });
            }
        }


        [HttpPost("upload-avatar/{id}")]
        public async Task<ActionResult> UploadAvatar(int id, [FromBody] UploadAvatarRequest request)
        {
            try
            {
                var user = await _context.Utilisateurs.FindAsync(id);
                if (user == null)
                    return NotFound();

                // Stocker seulement le nom du fichier ou une URL, pas la base64 complète
                user.Avatar = $"avatar_{id}.jpg"; // Ou stocker en fichier et sauvegarder le chemin

                await _context.SaveChangesAsync();
                return Ok(new { message = "Avatar mis à jour" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null) return NotFound();

            _context.Utilisateurs.Remove(utilisateur);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }


}