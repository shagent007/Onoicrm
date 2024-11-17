namespace Onoicrm.Domain.Entities;

public class BookingGroup : Entity
{
    public DateTime StartDate { get; set; } = DateTime.Now.ToUniversalTime();
    public DateTime? EndDate { get; set; }
    public int Price { get; set; }
    public long? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
    public long? PatientId { get; set; }
    public Patient? Patient { get; set; }
    protected override int GetClassId() => ClassNames.BookingGroup;
}