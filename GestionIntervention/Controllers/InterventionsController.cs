using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionIntervention.Data;
using GestionIntervention.Models;
using GestionIntervention.Services;



namespace GestionIntervention.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterventionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InterventionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Intervention>>> GetInterventions()
        {
            return await _context.Interventions
                .Include(i => i.Client)
                .Include(i => i.Produit)
                .Include(i => i.Incident)
                .Include(i => i.Technicien)
                .Include(i => i.Rapports)
                .Include(i => i.Images)
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Intervention>> GetIntervention(int id)
        {
            Console.WriteLine($"🔍 Chargement intervention {id}");

            var intervention = await _context.Interventions
                .Include(i => i.Client)
                .Include(i => i.Produit)
                .Include(i => i.Incident)  // D'abord Include l'incident
                .Include(i => i.Technicien)
                .Include(i => i.Rapports)
                .Include(i => i.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            if (intervention == null)
            {
                Console.WriteLine($"❌ Intervention {id} non trouvée");
                return NotFound();
            }

            // DEBUG
            Console.WriteLine($"✅ Intervention {intervention.Id} trouvée");
            Console.WriteLine($"🔍 IncidentId: {intervention.IncidentId}");
            Console.WriteLine($"🔍 Incident object: {intervention.Incident != null}");

            // Chargez explicitement les images de l'incident si besoin
            if (intervention.Incident != null)
            {
                Console.WriteLine($"🔍 Chargement des images pour l'incident {intervention.Incident.Id}");

                // Chargez les images de l'incident
                await _context.Entry(intervention.Incident)
                    .Collection(i => i.Images)
                    .LoadAsync();

                Console.WriteLine($"🔍 Images incident chargées: {intervention.Incident.Images?.Count ?? 0}");
            }

            return intervention;
        }

        [HttpPost]
        public async Task<ActionResult<Intervention>> PostIntervention(Intervention intervention)
        {
            // 1️⃣ Validation des données
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _context.Clients.AnyAsync(c => c.Id == intervention.ClientId))
                return BadRequest("Client non trouvé");

            if (!await _context.Produits.AnyAsync(p => p.Id == intervention.ProduitId))
                return BadRequest("Produit non trouvé");

            var technicien = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Id == intervention.TechnicienId && u.Role.ToLower() == "technicien");

            if (technicien == null)
                return BadRequest("Technicien non trouvé ou rôle invalide");

            if (intervention.IncidentId.HasValue &&
                !await _context.Incidents.AnyAsync(i => i.Id == intervention.IncidentId.Value))
                return BadRequest("Incident non trouvé");

            // 2️⃣ Ajouter l'intervention
            _context.Interventions.Add(intervention);
            await _context.SaveChangesAsync(); // L'ID est généré ici

            // 3️⃣ Charger les relations nécessaires pour l'email
            await _context.Entry(intervention).Reference(i => i.Client).LoadAsync();
            await _context.Entry(intervention).Reference(i => i.Produit).LoadAsync();

            // 4️⃣ Envoyer email au technicien assigné
            if (!string.IsNullOrEmpty(technicien.Email))
            {
                var emailService = HttpContext.RequestServices.GetRequiredService<EmailService>();

                string subject = $"Nouvelle intervention assignée (#{intervention.Id})";
                string body = $@"
Bonjour {technicien.Prenom},<br/>
Une nouvelle intervention a été assignée :<br/>
- Client : {intervention.Client?.Nom}<br/>
- Produit : {intervention.Produit?.Nom}<br/>
- Description : {intervention.Description}<br/>
Date planifiée : {intervention.DatePlanifiee:dd/MM/yyyy HH:mm}<br/><br/>
Merci.";

                await emailService.SendEmailAsync(technicien.Email, subject, body);
            }

            return CreatedAtAction(nameof(GetIntervention), new { id = intervention.Id }, intervention);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntervention(int id, Intervention intervention)
        {
            if (id != intervention.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(intervention).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterventionExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // Ajoutez cette méthode dans votre InterventionsController.cs
        [HttpGet("{id}/images")]
        public async Task<ActionResult<List<InterventionImage>>> GetInterventionImages(int id)
        {
            var images = await _context.InterventionImages
                .Where(img => img.InterventionId == id)
                .ToListAsync();

            return Ok(images);
        }

        // Endpoint proxy pour servir les images via l'API (contourne CORS)
        [HttpGet("serve-image")]
        public IActionResult ServeImage([FromQuery] string filename)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "interventions", filename);

                if (!System.IO.File.Exists(filePath))
                    return NotFound($"Image non trouvée: {filename}");

                var imageBytes = System.IO.File.ReadAllBytes(filePath);
                var contentType = "image/png"; // ou détection automatique du type

                return File(imageBytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur serveur: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntervention(int id)
        {
            var intervention = await _context.Interventions
                .Include(i => i.Images)
                .Include(i => i.Rapports)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (intervention == null)
                return NotFound();

            // Supprimer les fichiers images associés
            if (intervention.Images != null)
            {
                foreach (var image in intervention.Images)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Chemin.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            _context.Interventions.Remove(intervention);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/demarrer")]
        public async Task<IActionResult> StartIntervention(int id)
        {
            try
            {
                var intervention = await _context.Interventions.FindAsync(id);
                if (intervention == null)
                    return NotFound();

                intervention.Statut = "En cours";
                intervention.DateDebut = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }


        [HttpPatch("{id}/terminer")]
        public async Task<IActionResult> FinishIntervention(int id)
        {
            try
            {
                var intervention = await _context.Interventions.FindAsync(id);
                if (intervention == null)
                    return NotFound();

                intervention.Statut = "Terminé";
                intervention.DateFin = DateTime.UtcNow;

                // Si l'intervention est liée à un incident, le marquer comme résolu
                if (intervention.IncidentId.HasValue)
                {
                    var incident = await _context.Incidents.FindAsync(intervention.IncidentId.Value);
                    if (incident != null)
                    {
                        incident.Statut = "Résolu"; // Correction de l'accent
                        incident.DateResolution = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }

        private bool InterventionExists(int id)
        {
            return _context.Interventions.Any(e => e.Id == id);
        }
    }
}