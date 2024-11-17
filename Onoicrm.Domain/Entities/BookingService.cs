namespace Onoicrm.Domain.Entities;

public class BookingService : Entity
{
    public long? BookingId { get; set; }
    public Booking? Booking { get; set; }
    public long? ServiceId { get; set; }
    public Service? Service { get; set; }
    protected override int GetClassId() => ClassNames.BookingService;
}