using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class NovinkyContext : DbContext
    {
        public NovinkyContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Novinka> Novinky { get; set; }
    }
}