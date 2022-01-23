namespace Pitstop.InvoiceService.Events;

public class MaintenanceJobPlanned : Event
{
    public readonly string JobId;
    public readonly (string Id, string Name, string TelephoneNumber) CustomerInfo;
    public readonly (string LicenseNumber, string Brand, string Type) VehicleInfo;
    public readonly string Description;

    public MaintenanceJobPlanned(Guid messageId, string jobId, (string Id, string Name, string TelephoneNumber) customerInfo,
        (string LicenseNumber, string Brand, string Type) vehicleInfo, string description) : base(messageId)
    {
        JobId = jobId;
        CustomerInfo = customerInfo;
        VehicleInfo = vehicleInfo;
        Description = description;
    }
}