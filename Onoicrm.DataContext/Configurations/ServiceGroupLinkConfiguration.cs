using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class ServiceGroupLinkConfiguration : IEntityTypeConfiguration<ServiceGroupLink>
{
    public void Configure(EntityTypeBuilder<ServiceGroupLink> builder)
    {
        builder.ToTable("ServiceGroupLink");
    }
}