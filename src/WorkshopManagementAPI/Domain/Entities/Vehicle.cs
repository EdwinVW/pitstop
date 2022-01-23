namespace Pitstop.WorkshopManagementAPI.Domain.Entities;

public class Vehicle : Entity<LicenseNumber>
{
    public string Brand { get; private set; }
    public string Type { get; private set; }
    public string OwnerId { get; private set; }

    public Vehicle(LicenseNumber licenseNumber, string brand, string type, string ownerId) : base(licenseNumber)
    {
        Brand = brand;
        Type = type;
        OwnerId = ownerId;
    }
}