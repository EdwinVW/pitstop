namespace Pitstop.NotificationService.Events;

public class MaintenanceJobPlanned : Event
{
    public readonly Guid JobId;
    public readonly DateTime StartTime;
    public readonly DateTime EndTime;
    public readonly (string Id, string Name, string TelephoneNumber) CustomerInfo;
    public readonly (string LicenseNumber, string Brand, string Type) VehicleInfo;
    public readonly string Description;

    public MaintenanceJobPlanned(Guid messageId, Guid jobId, DateTime startTime, DateTime endTime,
        (string Id, string Name, string TelephoneNumber) customerInfo,
        (string LicenseNumber, string Brand, string Type) vehicleInfo,
        string description) : base(messageId)
    {
        JobId = jobId;
        StartTime = startTime;
        EndTime = endTime;
        CustomerInfo = customerInfo;
        VehicleInfo = vehicleInfo;
        Description = description;
    }
}