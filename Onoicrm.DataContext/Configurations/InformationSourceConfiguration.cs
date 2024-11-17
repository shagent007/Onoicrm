using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class InformationSourceConfiguration : IEntityTypeConfiguration<InformationSource>
{
    public void Configure(EntityTypeBuilder<InformationSource> builder)
    {
        builder.ToTable("InformationSource");
    }
}