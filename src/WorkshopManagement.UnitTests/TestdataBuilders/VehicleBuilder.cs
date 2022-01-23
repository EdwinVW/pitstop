namespace Pitstop.WorkshopManagement.UnitTests.TestdataBuilders;

public class VehicleBuilder
{
    private Random _rnd;

    public LicenseNumber LicenseNumber { get; private set; }
    public string Brand { get; private set; }
    public string Type { get; private set; }
    public string OwnerId { get; private set; }

    public VehicleBuilder()
    {
        _rnd = new Random();
        SetDefaults();
    }

    public VehicleBuilder WithLicenseNumber(string licenseNumber)
    {
        LicenseNumber = LicenseNumber.Create(licenseNumber);
        return this;
    }

    public VehicleBuilder WithRandomLicenseNumber()
    {
        LicenseNumber = LicenseNumber.Create(TestDataGenerators.GenerateRandomLicenseNumber());
        return this;
    }

    public VehicleBuilder WithBrand(string brand)
    {
        Brand = brand;
        return this;
    }

    public VehicleBuilder WithType(string type)
    {
        Type = type;
        return this;
    }

    public VehicleBuilder WithOwnerId(string ownerId)
    {
        OwnerId = ownerId;
        return this;
    }

    public Vehicle Build()
    {
        if (string.IsNullOrEmpty(OwnerId))
        {
            throw new InvalidOperationException("You must specify an owner id using the 'WithOwnerId' method.");
        }
        return new Vehicle(LicenseNumber, Brand, Type, OwnerId);
    }

    private void SetDefaults()
    {
        LicenseNumber = LicenseNumber.Create(TestDataGenerators.GenerateRandomLicenseNumber());
        Brand = "Volkswagen";
        Type = "Tiguan";
    }
}