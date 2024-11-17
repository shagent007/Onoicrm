using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onoicrm.Domain.Entities;

namespace Onoicrm.DataContext.Configurations;

public class AttachedFileConfiguration : IEntityTypeConfiguration<AttachedFile>
{
    public void Configure(EntityTypeBuilder<AttachedFile> builder)
    {
        builder.Ignore(e => e.Base64);
        builder.ToTable("AttachedFiles");
    }
}