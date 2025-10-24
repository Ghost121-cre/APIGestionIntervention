using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionIntervention.Data;
using GestionIntervention.Models;

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
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Intervention>> GetIntervention(int id)
        {
            var intervention = await _context.Interventions
                .Include(i => i.Client)
                .Include(i => i.Produit)
                .Include(i => i.Incident)
                .Include(i => i.Technicien)
                .Include(i => i.Rapports)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (intervention == null)
                return NotFound();

            return intervention;
        }

        [HttpPost]
        public async Task<ActionResult<Intervention>> PostIntervention(Intervention intervention)
        {
            _context.Interventions.Add(intervention);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIntervention), new { id = intervention.Id }, intervention);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntervention(int id, Intervention intervention)
        {
            if (id != intervention.Id)
                return BadRequest();

            _context.Entry(intervention).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntervention(int id)
        {
            var intervention = await _context.Interventions.FindAsync(id);
            if (intervention == null)
                return NotFound();

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
                        incident.Statut = "résolu";
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
    }
}