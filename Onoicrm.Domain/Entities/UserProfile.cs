using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Onoicrm.Domain.Entities;
public enum SalaryType {Salary, Percent, ServicePercent }
public enum Gender { Male, Female, NotDefined }
public class UserProfile: Entity
{
    private string? _fullName { get; set; }
    [StringLength(200)] 
    public string FirstName { get; set; } = "";
    [StringLength(200)] 
    public string LastName { get; set; } = "";
    public string? Patronymic { get; set; }
    public string PhoneNumber { get; set; } = "";
    public string WhatsAppNumber { get; set; } = "";
    public string JobPosition { get; set; } = "";
    public string Address { get; set; } = "";
    public string Description { get; set; } = "";
    public Gender GenderId { get; set; } = Gender.NotDefined;
    
    public long? ClinicId { get; set; }
    public long Salary { get; set; }
    public SalaryType SalaryType { get; set; } = SalaryType.Percent;
    public bool MailingConsent { get; set; }
    public string FullName
    {
        get => !string.IsNullOrEmpty(_fullName) ? _fullName : $"{LastName} {FirstName} {Patronymic}";
        set => _fullName = value;
    }

    public InformationSource? InformationSource { get; set; }
    public long? InformationSourceId { get; set; }
    
    public string? UserId { get; set; }
    [JsonIgnore]
    public IdentityUser? User { get; set; }
    public DateTime? BirthDate { get; set; }
    public IEnumerable<string> Roles { get; set; } = new List<string>();
    public JsonDocument? Data { get; set; }
    protected override int GetClassId() => ClassNames.UserProfile;
    public int Debit { get; set; }
    public int Credit { get; set; }
    public int Discount { get; set; }

    public long GetSalary(long sum)
    {
        switch (SalaryType)
        {
            case SalaryType.Percent: return sum * Salary / 100;
            default: throw new ArgumentOutOfRangeException();
        }
    }
}