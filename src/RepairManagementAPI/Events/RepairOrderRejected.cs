namespace Pitstop.RepairManagementAPI.Events;

public class RepairOrderRejected : Event
{
    public readonly string RepairOrderId;
    public readonly string RejectReason;

    public RepairOrderRejected(Guid messageId, string repairOrderId, string rejectReason
    )
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        RejectReason = rejectReason;
    }

    public static RepairOrderRejected FromCommand(RejectRepairOrder command)
    {
        return new RepairOrderRejected(
            Guid.NewGuid(),
            command.RepairOrderId,
            command.RejectReason
        );
    }
}