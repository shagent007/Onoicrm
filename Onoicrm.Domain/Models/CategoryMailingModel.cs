namespace Onoicrm.Domain.Models;

public class CategoryMailingModel
{
    public List<long?> Categories { get; set; } = new();
    public string Text { get; set; } = ""; 
    public long ClinicId { get; set; }

}