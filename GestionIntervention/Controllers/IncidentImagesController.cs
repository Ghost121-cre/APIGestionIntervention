using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionIntervention.Data;
using GestionIntervention.Models;
using Microsoft.AspNetCore.Hosting;

[Route("api/incidents/{incidentId}/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public ImagesController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpPost]
    public async Task<ActionResult<IncidentImage>> UploadImage(int incidentId, IFormFile file)
    {
        try
        {
            Console.WriteLine($"📸 Début upload image pour incident {incidentId}");

            // Vérifier que l'incident existe
            var incident = await _context.Incidents.FindAsync(incidentId);
            if (incident == null)
            {
                Console.WriteLine($"❌ Incident {incidentId} non trouvé");
                return NotFound($"Incident {incidentId} non trouvé");
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

            // Chemin de sauvegarde garanti
            var uploadsFolder = Path.Combine("wwwroot", "uploads", "incidents");
            var fullUploadsPath = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder);

            Console.WriteLine($"📁 Chemin de sauvegarde: {fullUploadsPath}");

            // Créer le dossier s'il n'existe pas
            if (!Directory.Exists(fullUploadsPath))
            {
                Directory.CreateDirectory(fullUploadsPath);
                Console.WriteLine($"📁 Dossier créé: {fullUploadsPath}");
            }

            // Créer un nom de fichier unique
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(fullUploadsPath, fileName);

            // Sauvegarder le fichier
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Créer l'entrée en base de données
            var incidentImage = new IncidentImage
            {
                IncidentId = incidentId,
                NomFichier = fileName,
                Chemin = $"/uploads/incidents/{fileName}",
                AltText = $"Image pour l'incident {incidentId}",
                DateCreation = DateTime.UtcNow
            };

            _context.IncidentImages.Add(incidentImage);
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Image uploadée avec ID: {incidentImage.Id}");

            return Ok(new
            {
                id = incidentImage.Id,
                nomFichier = incidentImage.NomFichier,
                chemin = incidentImage.Chemin,
                dateCreation = incidentImage.DateCreation
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
    public async Task<ActionResult<IEnumerable<IncidentImage>>> GetImages(int incidentId)
    {
        try
        {
            var images = await _context.IncidentImages
                .Where(img => img.IncidentId == incidentId)
                .ToListAsync();

            return Ok(images);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Erreur récupération images: {ex.Message}");
            return StatusCode(500, $"Erreur interne: {ex.Message}");
        }
    }
}