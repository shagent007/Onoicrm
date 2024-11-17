using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patient");
        builder.HasOne<Clinic>()
            .WithMany()
            .HasForeignKey(up => up.ClinicId) // Свойство UserId в UserProfile
            .IsRequired(false); 
    }
}