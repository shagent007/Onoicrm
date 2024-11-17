using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class BookingFileConfiguration : IEntityTypeConfiguration<BookingFile>
{
    public void Configure(EntityTypeBuilder<BookingFile> builder)
    {
        builder.Ignore(e => e.Base64);
        builder.ToTable("BookingFiles");
    }
}