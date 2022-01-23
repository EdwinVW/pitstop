namespace Pitstop.WorkshopManagement.UnitTests.TestdataBuilders;

public class MaintenanceJobFinishedEventBuilder
{
    public Guid JobId { get; private set; }
    public DateTime ActualStartTime { get; private set; }
    public DateTime ActualEndTime { get; private set; }
    public string Notes { get; private set; }

    public MaintenanceJobFinishedEventBuilder()
    {
    }

    public MaintenanceJobFinishedEventBuilder WithJobId(Guid jobId)
    {
        JobId = jobId;
        return this;
    }

    public MaintenanceJobFinishedEventBuilder WithActualStartTime(DateTime startTime)
    {
        ActualStartTime = startTime;
        return this;
    }

    public MaintenanceJobFinishedEventBuilder WithActualEndTime(DateTime endTime)
    {
        ActualEndTime = endTime;
        return this;
    }

    public MaintenanceJobFinishedEventBuilder WithNotes(string notes)
    {
        Notes = Notes;
        return this;
    }

    public MaintenanceJobFinished Build()
    {
        MaintenanceJobFinished e = new MaintenanceJobFinished(
            Guid.NewGuid(), JobId, ActualStartTime, ActualEndTime, Notes);

        return e;
    }
}