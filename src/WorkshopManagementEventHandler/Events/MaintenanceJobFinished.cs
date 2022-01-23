namespace Pitstop.WorkshopManagementEventHandler.Events;

public class MaintenanceJobFinished : Event
{
    public readonly Guid JobId;
    public readonly DateTime StartTime;
    public readonly DateTime EndTime;
    public readonly string Notes;

    public MaintenanceJobFinished(Guid messageId, Guid jobId, DateTime startTime, DateTime endTime, string notes) :
        base(messageId)
    {
        JobId = jobId;
        StartTime = startTime;
        EndTime = endTime;
        Notes = notes;
    }
}