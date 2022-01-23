namespace Pitstop.InvoiceService.Events;

public class MaintenanceJobFinished : Event
{
    public readonly string JobId;
    public readonly DateTime StartTime;
    public readonly DateTime EndTime;

    public MaintenanceJobFinished(Guid messageId, string jobId, DateTime startTime, DateTime endTime) :
        base(messageId)
    {
        JobId = jobId;
        StartTime = startTime;
        EndTime = endTime;
    }
}