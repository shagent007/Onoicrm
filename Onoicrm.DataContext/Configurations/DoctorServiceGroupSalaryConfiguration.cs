using Onoicrm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Onoicrm.DataContext.Configurations;

public class DoctorServiceGroupSalaryConfiguration : IEntityTypeConfiguration<DoctorServiceGroupSalary>
{
    public void Configure(EntityTypeBuilder<DoctorServiceGroupSalary> builder)
    {
        builder.ToTable("DoctorServiceGroupSalary");
        builder.HasOne<Clinic>()
            .WithMany()
            .HasForeignKey(up => up.ClinicId);
    }
}