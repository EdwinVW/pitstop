using Pitstop.TestUtils;

namespace Pitstop.NotificationService.UnitTests.TestdataBuilders;

/// <summary>
/// Builds a <see cref="Pitstop.NotificationService.Model.MaintenanceJob"/>.
/// Defaults represent a job planned for today.
/// </summary>
public class MaintenanceJobBuilder
{
    public string JobId { get; private set; }
    public string CustomerId { get; private set; }
    public string LicenseNumber { get; private set; }
    public DateTime StartTime { get; private set; }
    public string Description { get; private set; }

    public MaintenanceJobBuilder()
    {
        SetDefaults();
    }

    public MaintenanceJobBuilder WithJobId(string jobId)
    {
        JobId = jobId;
        return this;
    }

    public MaintenanceJobBuilder WithCustomerId(string customerId)
    {
        CustomerId = customerId;
        return this;
    }

    public MaintenanceJobBuilder WithLicenseNumber(string licenseNumber)
    {
        LicenseNumber = licenseNumber;
        return this;
    }

    public MaintenanceJobBuilder WithStartTime(DateTime startTime)
    {
        StartTime = startTime;
        return this;
    }

    public MaintenanceJobBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public Pitstop.NotificationService.Model.MaintenanceJob Build()
    {
        return new Pitstop.NotificationService.Model.MaintenanceJob
        {
            JobId = JobId,
            CustomerId = CustomerId,
            LicenseNumber = LicenseNumber,
            StartTime = StartTime,
            Description = Description
        };
    }

    private void SetDefaults()
    {
        JobId = TestDataPrimitives.RandomGuid();
        CustomerId = TestDataPrimitives.RandomGuid();
        LicenseNumber = TestDataPrimitives.GenerateRandomLicenseNumber();
        StartTime = TestDataPrimitives.RandomTimeslot().Start;
        Description = TestDataPrimitives.RandomDescription();
    }
}
