namespace Onoicrm.Domain.Entities;

public class TreatmentPlan : Entity
{
    public long ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
    public long PatientId { get; set; }
    public UserProfile? Patient { get; set; }
    public long DoctorId { get; set; }
    public UserProfile? Doctor { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now.ToUniversalTime();
    public long ToothId { get; set; }
    public Tooth? Tooth { get; set; }
    public string Caption { get; set; } = "";
    public string Description { get; set; } = "";
    protected override int GetClassId() => ClassNames.TreatmentPlan;
}