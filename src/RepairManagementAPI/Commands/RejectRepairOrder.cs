namespace Pitstop.RepairManagementAPI.Commands;

public class RejectRepairOrder : Command
{
    public readonly Guid RepairOrderId;
    public readonly DateTime RejectedAt;
    public readonly string RejectReason;

    public RejectRepairOrder(Guid messageId, Guid repairOrderId,DateTime rejectedAt, string rejectReason )
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        RejectedAt = rejectedAt;
        RejectReason = rejectReason;
    }
}