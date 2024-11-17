namespace Onoicrm.Domain.Entities;

public class Patient : Entity
{
    public string WhatsAppNumber { get; set; } = "";
    public string? JobPosition { get; set; } = "";
    public string? Address { get; set; } = "";
    public string? Description { get; set; } = "";
    public Gender GenderId { get; set; } = Gender.NotDefined;
    public bool? MailingConsent { get; set; }
    public string FullName { get; set; }
    public long? UserProfileId { get; set; }
    public long? ClinicId { get; set; }

    public InformationSource? InformationSource { get; set; }
    public long? InformationSourceId { get; set; }
    public DateTime? BirthDate { get; set; }
    protected override int GetClassId() => ClassNames.Patient;
}