public class RepairDecision
{
    public int RepairId { get; set; }
    public int CustomerId { get; set; }
    public string Decision { get; set; }
    public DateTime DecisionTimestamp { get; set; }

    public RepairDecision(int repairId, int customerId, string decision)
    {
        RepairId = repairId;
        CustomerId = customerId;
        Decision = decision;
        DecisionTimestamp = DateTime.Now;
    }
}
