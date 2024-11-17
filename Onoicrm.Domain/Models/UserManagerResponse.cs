using Onoicrm.Domain.Entities;

namespace Onoicrm.Domain.Models;

public class UserManagerResponse
{
    public string? Token { get; set; }
    public ICollection<string> Roles { get; set; } = new List<string>();
    public ICollection<string> Errors { get; set; } = new List<string>();
    public UserProfile? Profile { get; set; }
    public Clinic? Clinic { get; set; }
}

public class WhatsAppMessage
{
    public string? Token { get; set; }
    public string? ProfileId { get; set; }
    public string? To { get; set; }
    public string? Text { get; set; }
}

public class MailingModel
{
    public long ClinicId { get; set; }
    public string Text { get; set; }
}