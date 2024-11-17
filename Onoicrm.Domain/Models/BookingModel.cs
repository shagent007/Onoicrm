using Onoicrm.Domain.Entities;

namespace Onoicrm.Domain.Models;


public class BookingModel
{
    public Booking Booking { get; set; }
    public List<string> Dates { get; set; } = new();
    public List<string> Times { get; set; } = new();
    public List<long> Services { get; set; } = new();
}