using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionIntervention.Data;
using GestionIntervention.Models;
using Microsoft.AspNetCore.Hosting;

namespace GestionIntervention.Controllers
{
    [Route("api/interventions/{interventionId}/[controller]")]
    [ApiController]
    public class InterventionImagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public InterventionImagesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost]
        public async Task<ActionResult<InterventionImage>> UploadImage(int interventionId, IFormFile file)
        {
            try
            {
                Console.WriteLine($"📸 Début upload image pour intervention {interventionId}");

                // Vérifier que l'intervention existe
                var intervention = await _context.Interventions.FindAsync(interventionId);
                if (intervention == null)
                {
                    Console.WriteLine($"❌ Intervention {interventionId} non trouvée");
                    return NotFound($"Intervention {interventionId} non trouvée");
                }

                // Validation du fichier
                if (file == null || file.Length == 0)
                    return BadRequest("Fichier vide");

                if (file.Length > 5 * 1024 * 1024) // 5MB
                    return BadRequest("Fichier trop volumineux (max 5MB)");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                    return BadRequest("Type de fichier non autorisé");

                // Création garantie des dossiers
                var basePath = Directory.GetCurrentDirectory();
                var wwwrootPath = Path.Combine(basePath, "wwwroot");

                // Créer wwwroot s'il n'existe pas
                if (!Directory.Exists(wwwrootPath))
                {
                    Directory.CreateDirectory(wwwrootPath);
                    Console.WriteLine($"📁 Dossier wwwroot créé: {wwwrootPath}");
                }

                var uploadsPath = Path.Combine(wwwrootPath, "uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                    Console.WriteLine($"📁 Dossier uploads créé: {uploadsPath}");
                }

                var interventionsUploadsPath = Path.Combine(uploadsPath, "interventions");
                if (!Directory.Exists(interventionsUploadsPath))
                {
                    Directory.CreateDirectory(interventionsUploadsPath);
                    Console.WriteLine($"📁 Dossier interventions créé: {interventionsUploadsPath}");
                }

                Console.WriteLine($"📁 Chemin de sauvegarde final: {interventionsUploadsPath}");

                // Créer un nom de fichier unique
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(interventionsUploadsPath, fileName);

                // Sauvegarder le fichier
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                Console.WriteLine($"💾 Fichier sauvegardé: {filePath}");

                // Créer l'entrée en base de données
                var interventionImage = new InterventionImage
                {
                    InterventionId = interventionId,
                    NomFichier = fileName,
                    Chemin = $"/uploads/interventions/{fileName}",
                    DateUpload = DateTime.UtcNow
                };

                _context.InterventionImages.Add(interventionImage);
                await _context.SaveChangesAsync();

                Console.WriteLine($"✅ Image uploadée avec ID: {interventionImage.Id}");

                return Ok(new
                {
                    id = interventionImage.Id,
                    nomFichier = interventionImage.NomFichier,
                    chemin = interventionImage.Chemin,
                    dateUpload = interventionImage.DateUpload
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Erreur upload image: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InterventionImage>>> GetImages(int interventionId)
        {
            try
            {
                var images = await _context.InterventionImages
                    .Where(img => img.InterventionId == interventionId)
                    .ToListAsync();

                return Ok(images);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Erreur récupération images: {ex.Message}");
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(int interventionId, int imageId)
        {
            try
            {
                var image = await _context.InterventionImages
                    .FirstOrDefaultAsync(img => img.Id == imageId && img.InterventionId == interventionId);

                if (image == null)
                    return NotFound();

                // Supprimer le fichier physique
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "interventions", image.NomFichier);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Supprimer de la base
                _context.InterventionImages.Remove(image);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Erreur suppression image: {ex.Message}");
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }
    }
}