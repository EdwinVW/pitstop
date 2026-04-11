namespace Pitstop.NotificationService.UnitTests.TestdataBuilders;

public class DayHasPassedEventBuilder
{
    public Pitstop.NotificationService.Events.DayHasPassed Build()
    {
        return new Pitstop.NotificationService.Events.DayHasPassed(Guid.NewGuid());
    }
}
