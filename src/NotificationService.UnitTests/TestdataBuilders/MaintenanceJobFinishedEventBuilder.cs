using Pitstop.TestUtils;

namespace Pitstop.NotificationService.UnitTests.TestdataBuilders;

public class MaintenanceJobFinishedEventBuilder
{
    public string JobId { get; private set; }

    public MaintenanceJobFinishedEventBuilder()
    {
        SetDefaults();
    }

    public MaintenanceJobFinishedEventBuilder WithJobId(string jobId)
    {
        JobId = jobId;
        return this;
    }

    public Pitstop.NotificationService.Events.MaintenanceJobFinished Build()
    {
        return new Pitstop.NotificationService.Events.MaintenanceJobFinished(
            Guid.NewGuid(), JobId);
    }

    private void SetDefaults()
    {
        JobId = TestDataPrimitives.RandomGuid();
    }
}
