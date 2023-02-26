using Microsoft.EntityFrameworkCore;

namespace PruebaTecnica.API
{
    public class RouletteGameContext : DbContext
    {
        public RouletteGameContext(DbContextOptions<RouletteGameContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
