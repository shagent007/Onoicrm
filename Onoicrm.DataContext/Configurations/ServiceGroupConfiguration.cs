using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class ServiceGroupConfiguration : IEntityTypeConfiguration<ServiceGroup>
{
    public void Configure(EntityTypeBuilder<ServiceGroup> builder)
    {
        builder.ToTable("ServiceGroup");
    }
}