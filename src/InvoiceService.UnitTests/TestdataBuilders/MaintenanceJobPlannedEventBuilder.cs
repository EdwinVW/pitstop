using Pitstop.TestUtils;

namespace Pitstop.InvoiceService.UnitTests.TestdataBuilders;

/// <summary>
/// Builds a <see cref="Pitstop.InvoiceService.Events.MaintenanceJobPlanned"/> event.
/// </summary>
public class MaintenanceJobPlannedEventBuilder
{
    public string JobId { get; private set; }
    public string CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public string CustomerTelephoneNumber { get; private set; }
    public string LicenseNumber { get; private set; }
    public string VehicleBrand { get; private set; }
    public string VehicleType { get; private set; }
    public string Description { get; private set; }

    public MaintenanceJobPlannedEventBuilder()
    {
        SetDefaults();
    }

    public MaintenanceJobPlannedEventBuilder WithJobId(string jobId)
    {
        JobId = jobId;
        return this;
    }

    public MaintenanceJobPlannedEventBuilder WithCustomerId(string customerId)
    {
        CustomerId = customerId;
        return this;
    }

    public MaintenanceJobPlannedEventBuilder WithCustomerName(string name)
    {
        CustomerName = name;
        return this;
    }

    public MaintenanceJobPlannedEventBuilder WithLicenseNumber(string licenseNumber)
    {
        LicenseNumber = licenseNumber;
        return this;
    }

    public MaintenanceJobPlannedEventBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public Pitstop.InvoiceService.Events.MaintenanceJobPlanned Build()
    {
        return new Pitstop.InvoiceService.Events.MaintenanceJobPlanned(
            Guid.NewGuid(),
            JobId,
            (CustomerId, CustomerName, CustomerTelephoneNumber),
            (LicenseNumber, VehicleBrand, VehicleType),
            Description);
    }

    private void SetDefaults()
    {
        JobId = TestDataPrimitives.RandomGuid();
        CustomerId = TestDataPrimitives.RandomGuid();
        CustomerName = TestDataPrimitives.RandomName();
        CustomerTelephoneNumber = TestDataPrimitives.RandomPhoneNumber();
        LicenseNumber = TestDataPrimitives.GenerateRandomLicenseNumber();
        VehicleBrand = TestDataPrimitives.RandomCarBrand();
        VehicleType = TestDataPrimitives.RandomCarType();
        Description = TestDataPrimitives.RandomDescription();
    }
}
