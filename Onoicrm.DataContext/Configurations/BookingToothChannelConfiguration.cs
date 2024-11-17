using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class BookingToothChannelConfiguration : IEntityTypeConfiguration<BookingToothChannel>
{
    public void Configure(EntityTypeBuilder<BookingToothChannel> builder)
    {
        builder.ToTable("BookingToothChannel");
    }
}