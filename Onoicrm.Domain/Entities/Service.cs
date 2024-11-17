namespace Onoicrm.Domain.Entities;

public class Service : Entity
{
    public string Caption { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public string Code { get; set; }    
    public decimal PriceFrom { get; set; }
    public decimal PriceTo { get; set; }
    
    public string? LabaratoryCaption { get; set; } = "";
    public decimal? LabaratoryPrice { get; set; }
    public long ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
    public List<ServiceGroupLink> Links { get; set; } = new();
    protected override int GetClassId() => ClassNames.Service;
}

