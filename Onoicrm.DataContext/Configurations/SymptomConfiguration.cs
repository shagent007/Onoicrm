using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onoicrm.Domain.Entities;

namespace Onoicrm.DataContext.Configurations;

public class SymptomConfiguration : IEntityTypeConfiguration<Symptom>
{
    public void Configure(EntityTypeBuilder<Symptom> builder)
    {
        builder.ToTable("Symptoms");
    }
}