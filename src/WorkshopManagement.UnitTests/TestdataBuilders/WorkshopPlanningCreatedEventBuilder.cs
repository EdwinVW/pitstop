namespace Pitstop.WorkshopManagement.UnitTests.TestdataBuilders;

public class WorkshopPlanningCreatedEventBuilder
{
    public DateTime Date { get; private set; }

    public WorkshopPlanningCreatedEventBuilder()
    {
        SetDefaults();
    }

    public WorkshopPlanningCreatedEventBuilder WithDate(DateTime date)
    {
        Date = date;
        return this;
    }

    public WorkshopPlanningCreated Build()
    {
        WorkshopPlanningCreated e = new WorkshopPlanningCreated(Guid.NewGuid(), Date);
        return e;
    }

    private void SetDefaults()
    {
        Date = DateTime.Today;
    }
}