namespace Pitstop.InvoiceService.UnitTests.TestdataBuilders;

public class DayHasPassedEventBuilder
{
    public Pitstop.InvoiceService.Events.DayHasPassed Build()
    {
        return new Pitstop.InvoiceService.Events.DayHasPassed(Guid.NewGuid());
    }
}
