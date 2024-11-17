using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onoicrm.Domain.Entities;

namespace Onoicrm.DataContext.Configurations;

public class ToothConfiguration : IEntityTypeConfiguration<Tooth>
{
    public void Configure(EntityTypeBuilder<Tooth> builder)
    {
        builder.ToTable("Teeth");
    }
}