namespace Onoicrm.Domain.Entities;

public class BookingToothFile : AttachedFile
{
    public long? BookingToothId { get; set; }
    public BookingTooth? BookingTooth { get; set; }
    protected override int GetClassId() => ClassNames.BookingFile;
}