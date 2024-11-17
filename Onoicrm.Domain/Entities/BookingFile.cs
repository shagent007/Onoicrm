namespace Onoicrm.Domain.Entities;

public class BookingFile : AttachedFile
{
    public long BookingId { get; set; }
    public Booking? Booking { get; set; }
    protected override int GetClassId() => ClassNames.BookingFile;
}