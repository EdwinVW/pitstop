public class RepairDecisionProcessed
{
    public int RepairId { get; set; }
    public string Decision { get; set; }

    public RepairDecisionProcessed(int repairId, string decision)
    {
        RepairId = repairId;
        Decision = decision;
    }

    public void Publish()
    {
        Console.WriteLine($"Event: RepairId {RepairId} has been {Decision}.");
    }
}
