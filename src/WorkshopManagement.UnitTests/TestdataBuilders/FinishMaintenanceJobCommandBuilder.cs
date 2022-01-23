namespace Pitstop.WorkshopManagement.UnitTests.TestdataBuilders;

public class FinishMaintenanceJobCommandBuilder
{
    public Guid JobId { get; private set; }
    public DateTime ActualStartTime { get; private set; }
    public DateTime ActualEndTime { get; private set; }
    public string Notes { get; private set; }

    public FinishMaintenanceJobCommandBuilder()
    {
        SetDefaults();
    }

    public FinishMaintenanceJobCommandBuilder WithJobId(Guid jobId)
    {
        JobId = jobId;
        return this;
    }

    public FinishMaintenanceJobCommandBuilder WithActualStartTime(DateTime startTime)
    {
        ActualStartTime = startTime;
        return this;
    }

    public FinishMaintenanceJobCommandBuilder WithActualEndTime(DateTime endTime)
    {
        ActualEndTime = endTime;
        return this;
    }

    public FinishMaintenanceJobCommandBuilder WithNotes(string notes)
    {
        Notes = notes;
        return this;
    }

    public FinishMaintenanceJob Build()
    {
        FinishMaintenanceJob command = new FinishMaintenanceJob(Guid.NewGuid(), JobId,
            ActualStartTime, ActualEndTime, Notes);
        return command;
    }

    private void SetDefaults()
    {
        JobId = Guid.NewGuid();
        ActualStartTime = DateTime.Today.AddHours(8);
        ActualEndTime = DateTime.Today.AddHours(11);
        Notes = $"Notes";
    }
}