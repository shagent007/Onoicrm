using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onoicrm.Domain.Entities;

namespace Onoicrm.DataContext.Configurations;

public class PriceConfiguration : IEntityTypeConfiguration<Price>
{
    public void Configure(EntityTypeBuilder<Price> builder)
    {
        builder.ToTable("Prices");
    }
}