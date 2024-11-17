using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onoicrm.Domain.Entities;

namespace Onoicrm.DataContext.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles")
            .HasOne(up => up.User)
            .WithMany()
            .HasForeignKey(up => up.UserId);
        
       builder.HasOne<Clinic>()
            .WithMany()
            .HasForeignKey(up => up.ClinicId) // Свойство UserId в UserProfile
            .IsRequired(false); 
    

        builder.Property<string>(up => up.PhoneNumber)
            .HasMaxLength(20);
        
        builder.Property<string>(up => up.WhatsAppNumber)
            .HasMaxLength(20);

        builder.Property<string>(up => up.Address)
            .HasMaxLength(200);
        
        
        
        builder.Ignore(up => up.Roles);
    }
}