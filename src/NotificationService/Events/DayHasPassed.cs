namespace Pitstop.NotificationService.Events;

public class DayHasPassed : Event
{
    public DayHasPassed(Guid messageId) : base(messageId)
    {
    }
}