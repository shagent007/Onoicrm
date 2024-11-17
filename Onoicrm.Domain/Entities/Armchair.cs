namespace Onoicrm.Domain.Entities;

public class Armchair : Entity
{
    public long? ClinicId { get; set; }
    public Clinic? Clinic { get; set; }

    public string Caption { get; set; } = "";
    public string Description { get; set; } = "";

    public string WorkTimeFrom { get; set; } = "";
    public string WorkTimeTo { get; set; } = "";
    protected override int GetClassId() => ClassNames.Armchair;
}