using Onoicrm.Domain.Entities;

namespace Onoicrm.Domain.Models;

public class BookingReportModel
{
    public List<BookingTooth> ForAddBookingTeeth { get; set; } = new();
    public List<BookingTooth> ForDeleteBookingTeeth { get; set; } = new();
    public List<BookingTooth> ForUpdateBookingTeeth { get; set; } = new();
    public List<ImplementedService> ForDeleteImplementedServices { get; set; } = new();
    public List<ImplementedService> ForAddImplementedServices { get; set; } = new();
    public List<BookingToothFile> ForDeleteBookingToothFiles { get; set; } = new();
    public List<BookingFile> ForDeleteBookingFiles { get; set; } = new();
    public List<BookingFile> ForAddBookingFiles { get; set; } = new();
}