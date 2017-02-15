using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApplication.Entities
{
    public class WebApplicationDbContext : IdentityDbContext<User>
    {
        public WebApplicationDbContext(DbContextOptions<WebApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Restaurant> Restaurants { get; set; }
    }
}