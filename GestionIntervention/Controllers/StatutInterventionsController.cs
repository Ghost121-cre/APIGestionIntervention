using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionIntervention.Data;
using GestionIntervention.Models;

namespace GestionIntervention.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatutInterventionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StatutInterventionsController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatutIntervention>>> GetStatuts() =>
            await _context.StatutInterventions.Include(s => s.Intervention).Include(s => s.Utilisateur).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<StatutIntervention>> GetStatut(int id)
        {
            var statut = await _context.StatutInterventions.Include(s => s.Intervention).Include(s => s.Utilisateur).FirstOrDefaultAsync(s => s.Id == id);
            return statut == null ? NotFound() : statut;
        }

        [HttpPost]
        public async Task<ActionResult<StatutIntervention>> PostStatut(StatutIntervention statut)
        {
            _context.StatutInterventions.Add(statut);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStatut), new { id = statut.Id }, statut);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatut(int id, StatutIntervention statut)
        {
            if (id != statut.Id) return BadRequest();
            _context.Entry(statut).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!_context.StatutInterventions.Any(e => e.Id == id)) return NotFound(); else throw; }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatut(int id)
        {
            var statut = await _context.StatutInterventions.FindAsync(id);
            if (statut == null) return NotFound();
            _context.StatutInterventions.Remove(statut);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
