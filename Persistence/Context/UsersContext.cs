using Microsoft.EntityFrameworkCore;
using WebApplication2.Persistence.Models;

namespace WebApplication2.Persistence.Context
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
        public DbSet<WebApplication2.Persistence.Models.Users> Users { get; set; } = default!;
    }
}
