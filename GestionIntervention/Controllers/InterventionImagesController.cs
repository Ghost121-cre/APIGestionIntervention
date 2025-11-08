using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace GestionIntervention.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterventionImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public InterventionImagesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // 📤 Endpoint pour uploader une image liée à une intervention
        [HttpPost("upload/{interventionId}")]
        public async Task<IActionResult> UploadImage(int interventionId, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Aucune image reçue.");

            // Dossier cible
            var uploadDir = Path.Combine(_env.WebRootPath, "uploads", "interventions");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            // Générer un nom de fichier unique
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(uploadDir, fileName);

            // Sauvegarde du fichier
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Ici tu peux sauvegarder en BDD le lien avec l'intervention si besoin

            return Ok(new
            {
                FileName = fileName,
                Url = $"{Request.Scheme}://{Request.Host}/api/InterventionImages/serve-image/{fileName}"
            });
        }

        // 📷 Endpoint pour servir une image d’intervention
        [HttpGet("serve-image/{filename}")]
        public IActionResult ServeImage(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return BadRequest("Nom de fichier manquant.");

            var filePath = Path.Combine(_env.WebRootPath, "uploads", "interventions", filename);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Image introuvable.");

            var contentType = GetContentType(filePath);
            var image = System.IO.File.OpenRead(filePath);
            return File(image, contentType);
        }

        // 🔍 Détecte le bon type MIME selon l’extension
        private string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}
