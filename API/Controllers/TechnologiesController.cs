using API.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechnologiesController : ControllerBase
    {
        private readonly ArykaContext _context;

        public TechnologiesController(ArykaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Technologie>>> GetTechnologies()
        {
            return await _context.Technologies.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Technologie>> GetTechnology(int id)
        {
            var technology = await _context.Technologies.FindAsync(id);

            if (technology == null)
            {
                return NotFound();
            }

            return technology;
        }

        [HttpPost]
        public async Task<ActionResult<Technologie>> CreateTechnology(Technologie technology)
        {
            _context.Technologies.Add(technology);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTechnology), new { id = technology.Id }, technology);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTechnology(int id, Technologie technology)
        {
            if (id != technology.Id)
            {
                return BadRequest();
            }

            _context.Entry(technology).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TechnologyExists(id))
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
        public async Task<IActionResult> DeleteTechnology(int id)
        {
            var technology = await _context.Technologies.FindAsync(id);

            if (technology == null)
            {
                return NotFound();
            }

            _context.Technologies.Remove(technology);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TechnologyExists(int id)
        {
            return _context.Technologies.Any(e => e.Id == id);
        }
    }
}
