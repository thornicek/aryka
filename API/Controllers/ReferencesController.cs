using API.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferencesController : ControllerBase
    {
        private readonly ArykaContext _context;

        public ReferencesController(ArykaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reference>>> GetReferences()
        {
            return await _context.References.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reference>> GetReference(int id)
        {
            var reference = await _context.References.FindAsync(id);

            if (reference == null)
            {
                return NotFound();
            }

            return reference;
        }

        [HttpPost]
        public async Task<ActionResult<Reference>> CreateReference(Reference reference)
        {
            _context.References.Add(reference);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReference), new { id = reference.Id }, reference);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReference(int id, Reference reference)
        {
            if (id != reference.Id)
            {
                return BadRequest();
            }

            _context.Entry(reference).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReferenceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReference(int id)
        {
            var reference = await _context.References.FindAsync(id);

            if (reference == null)
            {
                return NotFound();
            }

            _context.References.Remove(reference);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReferenceExists(int id)
        {
            return _context.References.Any(e => e.Id == id);
        }
    }
}