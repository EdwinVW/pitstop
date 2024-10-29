namespace Pitstop.RepairManagementAPI.Events;

public class RepairOrderRejected : Event
{
    public readonly Guid RepairOrderId;
    public readonly DateTime RejectedAt;
    public readonly string RejectReason;

    public RepairOrderRejected(Guid messageId, Guid repairOrderId, string rejectReason, DateTime rejectedAt
    )
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        RejectReason = rejectReason;
        RejectedAt = rejectedAt;
    }

    public static RepairOrderRejected FromCommand(RejectRepairOrder command)
    {
        return new RepairOrderRejected(
            Guid.NewGuid(),
            command.RepairOrderId,
            command.RejectReason,
            command.RejectedAt
        );
    }
}