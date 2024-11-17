using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class BookingGroupConfiguration : IEntityTypeConfiguration<BookingGroup>
{
    public void Configure(EntityTypeBuilder<BookingGroup> builder)
    {
        builder.ToTable("BookingGroup");
    }
}