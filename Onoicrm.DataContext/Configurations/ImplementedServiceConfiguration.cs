using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class ImplementedServiceConfiguration : IEntityTypeConfiguration<ImplementedService>
{
    public void Configure(EntityTypeBuilder<ImplementedService> builder)
    {
        builder.ToTable("ImplementedService");
    }
}