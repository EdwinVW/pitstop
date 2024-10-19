namespace Pitstop.RepairManagementAPI.Events;

public class RepairOrderApproved : Event
{
    public readonly string RepairOrderId;

    public RepairOrderApproved(Guid messageId, string repairOrderId)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
    }

    public static RepairOrderApproved FromCommand(string repairOrderId)
    {
        return new RepairOrderApproved(
            Guid.NewGuid(),
            repairOrderId
        );
    }
}