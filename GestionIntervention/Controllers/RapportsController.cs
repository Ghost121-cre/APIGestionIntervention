using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionIntervention.Data;
using GestionIntervention.Models;

namespace GestionIntervention.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RapportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RapportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rapport>>> GetRapports()
        {
            var rapports = await _context.Rapports
                .Include(r => r.Intervention!)
                .ThenInclude(i => i!.Client)
                .ToListAsync();

            return Ok(rapports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rapport>> GetRapport(int id)
        {
            var rapport = await _context.Rapports
                .Include(r => r.Intervention!)
                .ThenInclude(i => i!.Client)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rapport == null)
                return NotFound(new { message = "Rapport non trouvé" });

            return Ok(rapport);
        }

        [HttpPost]
        public async Task<ActionResult<Rapport>> PostRapport(Rapport rapport)
        {
            // Validation des données
            if (rapport == null)
                return BadRequest(new { message = "Les données du rapport sont invalides" });

            if (string.IsNullOrWhiteSpace(rapport.Description))
                return BadRequest(new { message = "La description est requise" });

            if (string.IsNullOrWhiteSpace(rapport.Client))
                return BadRequest(new { message = "Le client est requis" });

            if (string.IsNullOrWhiteSpace(rapport.Intervenant))
                return BadRequest(new { message = "L'intervenant est requis" });

            if (string.IsNullOrWhiteSpace(rapport.TypeIntervention))
                return BadRequest(new { message = "Le type d'intervention est requis" });

            try
            {
                _context.Rapports.Add(rapport);
                await _context.SaveChangesAsync();

                // Recharger le rapport avec les relations
                var rapportAvecRelations = await _context.Rapports
                    .Include(r => r.Intervention!)
                    .ThenInclude(i => i!.Client)
                    .FirstOrDefaultAsync(r => r.Id == rapport.Id);

                return CreatedAtAction(nameof(GetRapport), new { id = rapport.Id }, rapportAvecRelations);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "Erreur lors de la création du rapport", error = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRapport(int id, Rapport rapport)
        {
            if (id != rapport.Id)
                return BadRequest(new { message = "ID du rapport incompatible" });

            if (rapport == null)
                return BadRequest(new { message = "Les données du rapport sont invalides" });

            // Vérifier si le rapport existe
            var rapportExistant = await _context.Rapports.FindAsync(id);
            if (rapportExistant == null)
                return NotFound(new { message = "Rapport non trouvé" });

            // Validation des données
            if (string.IsNullOrWhiteSpace(rapport.Description))
                return BadRequest(new { message = "La description est requise" });

            if (string.IsNullOrWhiteSpace(rapport.Client))
                return BadRequest(new { message = "Le client est requis" });

            if (string.IsNullOrWhiteSpace(rapport.Intervenant))
                return BadRequest(new { message = "L'intervenant est requise" });

            if (string.IsNullOrWhiteSpace(rapport.TypeIntervention))
                return BadRequest(new { message = "Le type d'intervention est requis" });

            try
            {
                // Mettre à jour les propriétés
                _context.Entry(rapportExistant).CurrentValues.SetValues(rapport);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RapportExists(id))
                    return NotFound(new { message = "Rapport non trouvé" });
                else
                    throw;
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "Erreur lors de la mise à jour du rapport", error = ex.InnerException?.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRapport(int id)
        {
            var rapport = await _context.Rapports.FindAsync(id);
            if (rapport == null)
                return NotFound(new { message = "Rapport non trouvé" });

            try
            {
                _context.Rapports.Remove(rapport);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "Erreur lors de la suppression du rapport", error = ex.InnerException?.Message });
            }
        }

        private bool RapportExists(int id)
        {
            return _context.Rapports.Any(e => e.Id == id);
        }
    }
}