using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionIntervention.Data;
using GestionIntervention.Models;

namespace GestionIntervention.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IncidentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Incident>>> GetIncidents()
        {
            return await _context.Incidents
                .Include(i => i.Client)
                .Include(i => i.Produit)
                .Include(i => i.Images)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetIncident(int id)
        {
            var incident = await _context.Incidents
                .Include(i => i.Client)
                .Include(i => i.Produit)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (incident == null)
                return NotFound();

            return incident;
        }

        [HttpPost]
        public async Task<ActionResult<Incident>> PostIncident(Incident incident)
        {
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIncident), new { id = incident.Id }, incident);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncident(int id, Incident incident)
        {
            if (id != incident.Id) return BadRequest();

            _context.Entry(incident).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null) return NotFound();

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/resoudre")]
        public async Task<IActionResult> MarkAsResolved(int id)
        {
            try
            {
                var incident = await _context.Incidents.FindAsync(id);
                if (incident == null)
                    return NotFound();

                incident.Statut = "résolu";
                incident.DateResolution = DateTime.UtcNow;

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
