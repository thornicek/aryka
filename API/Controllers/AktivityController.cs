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
    public class AktivityController : ControllerBase
    {
        private readonly ArykaContext _context;

        public AktivityController(ArykaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Aktivita>>> GetActivities([FromQuery] int page = 1, [FromQuery] int pageSize = 2)
        {
            var skipAmount = (page - 1) * pageSize;

            var activities = await _context.Aktivity
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();

            return activities;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Aktivita>> GetActivity(int id)
        {
            var activity = await _context.Aktivity.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        [HttpPost]
        public async Task<ActionResult<Aktivita>> CreateActivity(Aktivita aktivita)
        {
            _context.Aktivity.Add(aktivita);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActivity), new { id = aktivita.Id }, aktivita);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateActivity(int id, Aktivita aktivita)
        {
            if (id != aktivita.Id)
            {
                return BadRequest();
            }

            _context.Entry(aktivita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
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
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var activity = await _context.Aktivity.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            _context.Aktivity.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActivityExists(int id)
        {
            return _context.Aktivity.Any(e => e.Id == id);
        }
    }
}
