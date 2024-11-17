using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Onoicrm.Domain.Entities;
public enum CompanyType {Clinic, Company}
public enum BookingType {Doctor, Armchair}
public class Clinic : Entity
{
    public string Caption { get; set; } = "";
    public DateTimeOffset ExpiredDate { get; set; } = DateTimeOffset.Now;
    protected override int GetClassId() =>  ClassNames.Clinic;
    public long? GroupUserProfileId { get; set; }
    [JsonIgnore] public string PasswordHash { get; set; } = "";
    public int MaxDoctorCount { get; set; }
    public int MaxChairCount { get; set; }
    public string? WappiProfileId { get; set; }
    public string? WappiToken { get; set; }
    public string? WorkStartTime { get; set; }
    public string? WorkEndTime { get; set; }
    public int BookingTimeDuration { get; set; }
    public bool UseArmchairForBooking { get; set; }
    public BookingType BookingType { get; set; } = BookingType.Doctor;
    

    public CompanyType Type { get; set; } = CompanyType.Clinic;
    [NotMapped]
    public bool HasWhatsAppAccount => !string.IsNullOrEmpty(WappiToken) && !string.IsNullOrEmpty(WappiProfileId);
}