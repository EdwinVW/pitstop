public class ProcessRepairDecision
{
    public int RepairId { get; set; }
    public int CustomerId { get; set; }
    public string Decision { get; set; } // "accepted" or "declined"

    // Constructor
    public ProcessRepairDecision(int repairId, int customerId, string decision)
    {
        RepairId = repairId;
        CustomerId = customerId;
        Decision = decision;
    }

    public void Execute()
    {
        UpdateRepairStatus();

        NotifyMechanic();
    }

    private void UpdateRepairStatus()
    {
        // Logic to update the repair status (either to accepted or declined)
        Console.WriteLine($"Updating repair status for RepairId {RepairId} to {Decision}");
        // Database logic here
    }

    private void NotifyMechanic()
    {
        // Logic to call NotificationService to notify the mechanic
        Console.WriteLine($"Notifying mechanic about the customer's {Decision} decision for RepairId {RepairId}");
        // Call the notification service here
    }
}
