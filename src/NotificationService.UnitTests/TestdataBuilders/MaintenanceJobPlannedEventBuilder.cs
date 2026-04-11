using Pitstop.TestUtils;

namespace Pitstop.NotificationService.UnitTests.TestdataBuilders;

public class MaintenanceJobPlannedEventBuilder
{
    public Guid JobId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
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

    public MaintenanceJobPlannedEventBuilder WithJobId(Guid jobId)
    {
        JobId = jobId;
        return this;
    }

    public MaintenanceJobPlannedEventBuilder WithStartTime(DateTime startTime)
    {
        StartTime = startTime;
        return this;
    }

    public MaintenanceJobPlannedEventBuilder WithEndTime(DateTime endTime)
    {
        EndTime = endTime;
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

    public Pitstop.NotificationService.Events.MaintenanceJobPlanned Build()
    {
        return new Pitstop.NotificationService.Events.MaintenanceJobPlanned(
            Guid.NewGuid(),
            JobId,
            StartTime,
            EndTime,
            (CustomerId, CustomerName, CustomerTelephoneNumber),
            (LicenseNumber, VehicleBrand, VehicleType),
            Description);
    }

    private void SetDefaults()
    {
        JobId = Guid.NewGuid();
        var timeslot = TestDataPrimitives.RandomTimeslot();
        StartTime = timeslot.Start;
        EndTime = timeslot.End;
        CustomerId = TestDataPrimitives.RandomGuid();
        CustomerName = TestDataPrimitives.RandomName();
        CustomerTelephoneNumber = TestDataPrimitives.RandomPhoneNumber();
        LicenseNumber = TestDataPrimitives.GenerateRandomLicenseNumber();
        VehicleBrand = TestDataPrimitives.RandomCarBrand();
        VehicleType = TestDataPrimitives.RandomCarType();
        Description = TestDataPrimitives.RandomDescription();
    }
}
