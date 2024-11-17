using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class BookingСancellationReasonConfiguration : IEntityTypeConfiguration<BookingCancellationReason>
{
    public void Configure(EntityTypeBuilder<BookingCancellationReason> builder)
    {
        builder.ToTable("BookingСancellationReason");
    }
}