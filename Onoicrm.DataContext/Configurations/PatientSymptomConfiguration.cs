using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onoicrm.Domain.Entities;

namespace Onoicrm.DataContext.Configurations;

public class PatientSymptomConfiguration : IEntityTypeConfiguration<PatientSymptom>
{
    public void Configure(EntityTypeBuilder<PatientSymptom> builder)
    {
        builder.ToTable("PatientSymptom");
    }
}