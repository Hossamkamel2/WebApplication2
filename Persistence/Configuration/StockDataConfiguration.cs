using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Persistence.Models;

namespace WebApplication2.Persistence.Configuration
{
    public class StockDataConfiguration : IEntityTypeConfiguration<StockData>
    {
        public void Configure(EntityTypeBuilder<StockData> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
