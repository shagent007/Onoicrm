using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onoicrm.Domain.Entities;

namespace Onoicrm.DataContext.Configurations;

public class ToothStateConfiguration : IEntityTypeConfiguration<ToothState>
{
    public void Configure(EntityTypeBuilder<ToothState> builder)
    {
        builder.ToTable("ToothStates");
    }
}