namespace Pitstop.RepairManagementAPI.Events;

public class RepairOrderApproved : Event
{
    public readonly Guid RepairOrderId;
    public readonly DateTime ApproveDate;

    public RepairOrderApproved(Guid messageId, Guid repairOrderId, DateTime approveDate)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        ApproveDate = approveDate;
    }

    public static RepairOrderApproved FromCommand(ApproveRepairOrder command)
    {
        return new RepairOrderApproved(
            Guid.NewGuid(),
            command.RepairOrderId,
            command.ApproveDate
        );
    }
}