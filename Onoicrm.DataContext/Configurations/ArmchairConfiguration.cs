using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onoicrm.Domain.Entities;

namespace Onoicrm.DataContext.Configurations;

public class ArmchairConfiguration : IEntityTypeConfiguration<Armchair>
{
    public void Configure(EntityTypeBuilder<Armchair> builder)
    {
        builder.ToTable("Armchairs");
    }
}