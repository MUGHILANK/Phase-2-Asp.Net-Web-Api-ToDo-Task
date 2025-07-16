using Microsoft.EntityFrameworkCore;
using todoTask.Models.Domain;

namespace todoTask.Data
{
    public class MKTodotaskDbContext : DbContext
    {
        public MKTodotaskDbContext(DbContextOptions<MKTodotaskDbContext> options) : base(options)
        {
        }

        public DbSet<Todotask> Todotasks { get; set; }
    }
}
