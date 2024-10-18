namespace Pitstop.NotificationService.Events;

public class MaintenanceJobStart : Event
{
    public readonly string JobId; 
    public readonly DateTime StartTime;
    public readonly DateTime EndTime;
    
    public MaintenanceJobStart(Guid messageId, string jobId, DateTime startTime, DateTime endTime) :
        base(messageId)
    {
        JobId = jobId;
        JobId = jobId;
        StartTime = startTime;
        EndTime = endTime;
    }
}
