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
        public async Task<ActionResult<List<Novinka>>> GetArticles()
        {
            var novinky = await _context.Novinky.ToListAsync();

            return novinky;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Novinka>> GetArticle(int id)
        {
            return await _context.Novinky.FindAsync(id);
        }
    }
}