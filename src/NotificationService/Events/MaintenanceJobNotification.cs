namespace Pitstop.NotificationService.Events;

public class MaintenanceJobNotification : Event
{
    
    
    public MaintenanceJobNotification(Guid messageId, string jobId) :
        base(messageId)
    {
        JobId = jobId;
    }
}
