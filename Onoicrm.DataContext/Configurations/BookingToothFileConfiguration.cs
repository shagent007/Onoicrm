using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class BookingToothFileConfiguration : IEntityTypeConfiguration<BookingToothFile>
{
    public void Configure(EntityTypeBuilder<BookingToothFile> builder)
    {
        builder.Ignore(e => e.Base64);
        builder.ToTable("BookingToothFile");
    }
}