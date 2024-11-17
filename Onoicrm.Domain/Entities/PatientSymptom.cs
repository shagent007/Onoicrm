namespace Onoicrm.Domain.Entities;

public class PatientSymptom : Entity
{
    public long? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
    public long? PatientId { get; set; }
    public Patient? Patient { get; set; }
    public long? SymptomId { get; set; }
    public Symptom? Symptom { get; set; }
    public bool Value { get; set; }
    protected override int GetClassId() => ClassNames.PatientSymptom;
}