using System.ComponentModel.DataAnnotations;

namespace Onoicrm.Domain.Entities;

public class BookingTooth : Entity
{
    [Required]
    public long ToothId { get; set; }
    public Tooth? Tooth { get; set; }
    [Required]
    public long BookingId { get; set; }
    public Booking? Booking { get; set; }
    public long? ToothStateId { get; set; }
    public ToothState? ToothState { get; set; }
    public string Caption { get; set; } = "";
    public string Description { get; set; } = "";
    public ICollection<BookingToothChannel> Channels { get; set; } = new List<BookingToothChannel>();
    public ICollection<BookingToothFile> Files { get; set; } = new List<BookingToothFile>();
    public ICollection<ImplementedService> ImplementedServices { get; set; } = new List<ImplementedService>();
    protected override int GetClassId() => ClassNames.ToothBooking;
}