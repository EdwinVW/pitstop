namespace Pitstop.WorkshopManagementEventHandler.Events;

public class VehicleRegistered : Event
{
    public readonly string LicenseNumber;
    public readonly string Brand;
    public readonly string Type;
    public readonly string OwnerId;

    public VehicleRegistered(Guid messageId, string licenseNumber, string brand, string type, string ownerId) :
        base(messageId)
    {
        LicenseNumber = licenseNumber;
        Brand = brand;
        Type = type;
        OwnerId = ownerId;
    }
}