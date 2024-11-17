using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onoicrm.Domain.Entities;

namespace Onoicrm.DataContext.Configurations;

public class BookingToothConfiguration : IEntityTypeConfiguration<BookingTooth>
{
    public void Configure(EntityTypeBuilder<BookingTooth> builder)
    {
        builder.ToTable("BookingTooth");
    }
}