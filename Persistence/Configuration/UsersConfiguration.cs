using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Persistence.Models;

namespace WebApplication2.Persistence.Configuration
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.EmailAddress).IsUnique();
        }
    }
}
