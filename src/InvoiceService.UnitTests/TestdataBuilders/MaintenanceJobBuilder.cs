using Pitstop.TestUtils;

namespace Pitstop.InvoiceService.UnitTests.TestdataBuilders;

/// <summary>
/// Builds a <see cref="Pitstop.InvoiceService.Model.MaintenanceJob"/>.
/// Defaults represent a finished job that is ready to be invoiced (StartTime, EndTime set, Finished=true).
/// </summary>
public class MaintenanceJobBuilder
{
    public string JobId { get; private set; }
    public string CustomerId { get; private set; }
    public string LicenseNumber { get; private set; }
    public string Description { get; private set; }
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public bool Finished { get; private set; }
    public bool InvoiceSent { get; private set; }

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

    public MaintenanceJobBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public MaintenanceJobBuilder WithStartTime(DateTime? startTime)
    {
        StartTime = startTime;
        return this;
    }

    public MaintenanceJobBuilder WithEndTime(DateTime? endTime)
    {
        EndTime = endTime;
        return this;
    }

    public MaintenanceJobBuilder WithFinished(bool finished)
    {
        Finished = finished;
        return this;
    }

    public MaintenanceJobBuilder WithInvoiceSent(bool invoiceSent)
    {
        InvoiceSent = invoiceSent;
        return this;
    }

    public Pitstop.InvoiceService.Model.MaintenanceJob Build()
    {
        return new Pitstop.InvoiceService.Model.MaintenanceJob
        {
            JobId = JobId,
            CustomerId = CustomerId,
            LicenseNumber = LicenseNumber,
            Description = Description,
            StartTime = StartTime,
            EndTime = EndTime,
            Finished = Finished,
            InvoiceSent = InvoiceSent
        };
    }

    private void SetDefaults()
    {
        JobId = TestDataPrimitives.RandomGuid();
        CustomerId = TestDataPrimitives.RandomGuid();
        LicenseNumber = TestDataPrimitives.GenerateRandomLicenseNumber();
        Description = TestDataPrimitives.RandomDescription();
        var timeslot = TestDataPrimitives.RandomTimeslot();
        StartTime = timeslot.Start;
        EndTime = timeslot.End;
        Finished = true;
        InvoiceSent = false;
    }
}
