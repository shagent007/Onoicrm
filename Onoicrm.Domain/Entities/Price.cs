namespace Onoicrm.Domain.Entities;

public class Price : Entity
{
    public long? BookingId { get; set; }
    public Booking? Booking { get; set; }
    public string Caption { get; set; } = "";
    public string Description { get; set; } = "";
    public int Value { get; set; }
    public Currency Currency { get; set; } = Currency.Som;
    protected override int GetClassId() => ClassNames.Price;
}