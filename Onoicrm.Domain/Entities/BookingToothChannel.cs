namespace Onoicrm.Domain.Entities;

public class BookingToothChannel : Entity
{
    public BookingTooth? BookingTooth { get; set; }
    public long BookingToothId { get; set; }
    public Channel? Channel { get; set; }
    public long ChannelId { get; set; }
    public long MasterCon { get; set; }
    public long MasterFile { get; set; }
    protected override int GetClassId() => ClassNames.Channel;
}