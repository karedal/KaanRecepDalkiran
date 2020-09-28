using Microsoft.EntityFrameworkCore;

namespace Sum.Api.Data
{
    public class SumDbContext : DbContext
    {
        public SumDbContext(DbContextOptions options): base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
    }
}