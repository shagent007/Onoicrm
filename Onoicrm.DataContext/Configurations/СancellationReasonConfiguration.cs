using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class СancellationReasonConfiguration : IEntityTypeConfiguration<CancellationReason>
{
    public void Configure(EntityTypeBuilder<CancellationReason> builder)
    {
        builder.ToTable("СancellationReason");
    }
}