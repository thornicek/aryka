using API.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly ArykaContext _context;
        public ArticlesController(ArykaContext context)
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

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            // Retrieve all articles
            var articles = await _context.Novinky.ToListAsync();

            // Remove all articles from the context
            _context.Novinky.RemoveRange(articles);

            // Save changes
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Find the article with the given id
            var article = await _context.Novinky.FindAsync(id);

            // If the article doesn't exist, return NotFound
            if (article == null)
            {
                return NotFound();
            }

            // Remove the article from the context
            _context.Novinky.Remove(article);

            // Save changes
            await _context.SaveChangesAsync();

            return NoContent();
    }

    }
}