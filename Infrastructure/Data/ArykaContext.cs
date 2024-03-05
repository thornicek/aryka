using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ArykaContext : DbContext
    {
        public ArykaContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Novinka> Novinky { get; set; }
        public DbSet<Technologie> Technologies {get; set;}
        public DbSet<Aktivita> Aktivity {get; set;}
        public DbSet<Reference> References {get; set;}
         
    }
}