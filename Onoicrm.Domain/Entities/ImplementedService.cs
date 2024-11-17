namespace Onoicrm.Domain.Entities;

public class ImplementedService : Entity
{
    public long ServiceId { get; set; }
    public Service? Service { get; set; }
    public long BookingId { get; set; }
    public Booking? Booking { get; set; }
    public long? BookingToothId { get; set; }
    public BookingTooth? BookingTooth { get; set; }
    public long? DoctorId { get; set; }
    public UserProfile? Doctor { get; set; }
    public long? ServiceGroupId { get; set; }
    public ServiceGroup? ServiceGroup { get; set; }
    public long LabaratoryPrice { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public string Caption { get; set; } = "";
    public string Code { get; set; }
    public string Description { get; set; } = "";
    public long Salary { get; set; }
    public SalaryType SalaryType { get; set; } = SalaryType.Percent;
    
    protected override int GetClassId() => ClassNames.ImplementedService;
}