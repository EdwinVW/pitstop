namespace Pitstop.TimeService.Events;

public class DayHasPassed : Event
{
    public DayHasPassed(Guid messageId) : base(messageId)
    {
    }
}