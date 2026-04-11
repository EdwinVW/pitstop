namespace Pitstop.InvoiceService.Model;

public class MaintenanceJob
{
    public string JobId { get; set; }
    public string LicenseNumber { get; set; }
    public string CustomerId { get; set; }
    public string Description { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool Finished { get; set; }
    public bool InvoiceSent { get; set; }

    public static MaintenanceJob CreateFrom(MaintenanceJobPlanned source) => new MaintenanceJob
    {
        JobId = source.JobId,
        CustomerId = source.CustomerInfo.Id,
        LicenseNumber = source.VehicleInfo.LicenseNumber,
        Description = source.Description
    };
}