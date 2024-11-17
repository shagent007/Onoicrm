using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onoicrm.Domain.Entities;

namespace Onoicrm.DataContext.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Groups")
            .HasOne(x=> x.Parent)
            .WithMany(x=> x.Children)
            .HasForeignKey(x=> x.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}