namespace Onoicrm.Domain.Models;

public class ListResponse
{
    public ListResponse(IEnumerable<object> items, long? total)
    {
        Items = items;
        Total = total;
    }

    public IEnumerable<object> Items { get; set; }
    public long? Total { get; set; }
}