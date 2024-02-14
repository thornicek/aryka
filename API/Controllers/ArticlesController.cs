using API.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController:ControllerBase
    {
        private readonly NovinkyContext _context;
        public ArticlesController(NovinkyContext context)
        {
            _context = context;
            
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Novinka>>> GetArticles([FromQuery] int page = 1, [FromQuery] int pageSize = 2)
        {
            var skipAmount = (page - 1) * pageSize;

            var novinky = await _context.Novinky
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();

            return novinky;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Novinka>> GetArticle(int id)
        {
            return await _context.Novinky.FindAsync(id);
        }
    
        [HttpPost]
        public async Task<ActionResult<Novinka>> CreateArticle(Novinka novinka)
        {
            _context.Novinky.Add(novinka);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticle), new { id = novinka.Id }, novinka);
        }

        
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, Novinka novinka)
        {
            if (id != novinka.Id)
            {
                return BadRequest();
            }

            _context.Entry(novinka).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
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

        private bool ArticleExists(int id)
        {
            return _context.Novinky.Any(e => e.Id == id);
        }
    }
}