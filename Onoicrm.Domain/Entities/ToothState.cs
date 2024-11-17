namespace Onoicrm.Domain.Entities;

public class ToothState : Entity
{
    public string Caption { get; set; } = "";
    public string Description { get; set; } = "";
    public string CssClass { get; set; } = "";
    public string Color { get; set; } = "";
    public long? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }
    protected override int GetClassId() => ClassNames.ToothState;
}