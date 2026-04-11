namespace Pitstop.NotificationService.Model;

public class MaintenanceJob
{
    public string JobId { get; set; }
    public string LicenseNumber { get; set; }
    public string CustomerId { get; set; }
    public DateTime StartTime { get; set; }
    public string Description { get; set; }

    public static MaintenanceJob CreateFrom(MaintenanceJobPlanned source) => new MaintenanceJob
    {
        JobId = source.JobId.ToString(),
        CustomerId = source.CustomerInfo.Id,
        LicenseNumber = source.VehicleInfo.LicenseNumber,
        StartTime = source.StartTime,
        Description = source.Description
    };
}