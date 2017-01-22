using Microsoft.EntityFrameworkCore;

namespace WebApplication.Entities
{
    public class WebApplicationDbContext : DbContext
    {
        public WebApplicationDbContext(DbContextOptions<WebApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Restaurant> Restaurants { get; set; }
    }
}