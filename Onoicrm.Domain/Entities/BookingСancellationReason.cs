namespace Onoicrm.Domain.Entities;

public class BookingCancellationReason :Entity
{
    public long BookingId { get; set; }
    public Booking? Booking { get; set; }
    public long CancellationReasonId { get; set; }
    public CancellationReason? CancellationReason { get; set; }
    public long ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
    
    protected override int GetClassId() => ClassNames.CancellationReason;
}