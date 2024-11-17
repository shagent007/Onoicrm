namespace Onoicrm.Domain.Entities;

public class Payment : Entity
{
    public DateTime Date { get; set; }
    public int Sum { get; set; } = 0;
    public string? Caption { get; set; }
    public string? Description { get; set; }
    public long? ProfileId { get; set; }
    public UserProfile? Profile { get; set; }
    public long? PatientId { get; set; }
    public Patient? Patient { get; set; }
    public long? BookingGroupId { get; set; }
    public BookingGroup? BookingGroup { get; set; }
    public string Method { get; set; }
    public long? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
    protected override int GetClassId() => ClassNames.Payment;
}