using FilteringPagination.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FilteringPagination.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Post> Posts { get; set; }
    }
}
