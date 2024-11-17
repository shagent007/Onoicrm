namespace Onoicrm.Domain.Entities;

public class CancellationReason :Entity
{
    public string Caption { get; set; }
    public string? Description { get; set; }
    protected override int GetClassId() => ClassNames.CancellationReason;
}