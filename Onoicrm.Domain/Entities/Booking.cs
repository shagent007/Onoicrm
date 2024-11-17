using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Onoicrm.Domain.Entities;

public enum DiscountType {Percent, Money}

public class Booking : Entity
{    
    public string Complaint { get; set; } = "";
    public string Caption { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime DateTimeStart { get; set; }
    public DateTime DateTimeEnd { get; set; }
    [Required]
    public long? DoctorId { get; set; }
    public UserProfile? Doctor { get; set; }
    public long? PatientId { get; set; }
    public Patient? Patient { get; set; }
    public long? ArmchairId { get; set; }
    public Armchair? Armchair { get; set; }
    [Required]
    public long? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }

    public long Salary { get; set; }
    public SalaryType SalaryType { get; set; } = SalaryType.Percent;
    
    public long? TreatmentPlanId { get; set; }
    public TreatmentPlan? TreatmentPlan { get; set; }
    public long Discount { get; set; }
    public DiscountType DiscountType { get; set; }
    public long? BookingGroupId { get; set; }
    public BookingGroup? BookingGroup { get; set; }
    
    public long? ServiceGroupId { get; set; }
    public ServiceGroup? ServiceGroup { get; set; }
    public ICollection<BookingFile> Files { get; set; } = new List<BookingFile>();
    public ICollection<BookingTooth> BookingTeeth { get; set; } = new List<BookingTooth>();
    public ICollection<ImplementedService> ImplementedServices { get; set; } = new List<ImplementedService>();
    [NotMapped]
    public string DateTime
    {
        get
        {
           var formattedDateTimeStart = DateTimeStart.AddHours(6).ToString("dd-MMMM HH:mm", CultureInfo.GetCultureInfo("ru-RU"));
           var formattedDateTimeEnd = DateTimeEnd.AddHours(6).ToString("HH:mm", CultureInfo.GetCultureInfo("ru-RU"));
           return $"{formattedDateTimeStart}-{formattedDateTimeEnd}"; 
        }
    }
    
    public decimal GetTotal(decimal sum) => DiscountType switch
    {
        DiscountType.Money => sum - Discount,
        DiscountType.Percent => Discount > 0 ? sum - sum * Discount / 100 : sum,
        _ => throw new ArgumentOutOfRangeException()
    };

    protected override int GetClassId() =>  ClassNames.Booking;
}