using Pitstop.TestUtils;

namespace Pitstop.InvoiceService.UnitTests.TestdataBuilders;

/// <summary>
/// Builds a <see cref="Pitstop.InvoiceService.Events.MaintenanceJobFinished"/> event.
/// </summary>
public class MaintenanceJobFinishedEventBuilder
{
    public string JobId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    public MaintenanceJobFinishedEventBuilder()
    {
        SetDefaults();
    }

    public MaintenanceJobFinishedEventBuilder WithJobId(string jobId)
    {
        JobId = jobId;
        return this;
    }

    public MaintenanceJobFinishedEventBuilder WithStartTime(DateTime startTime)
    {
        StartTime = startTime;
        return this;
    }

    public MaintenanceJobFinishedEventBuilder WithEndTime(DateTime endTime)
    {
        EndTime = endTime;
        return this;
    }

    public Pitstop.InvoiceService.Events.MaintenanceJobFinished Build()
    {
        return new Pitstop.InvoiceService.Events.MaintenanceJobFinished(
            Guid.NewGuid(), JobId, StartTime, EndTime);
    }

    private void SetDefaults()
    {
        JobId = TestDataPrimitives.RandomGuid();
        var timeslot = TestDataPrimitives.RandomTimeslot();
        StartTime = timeslot.Start;
        EndTime = timeslot.End;
    }
}
