namespace Pitstop.CustomerSupportAPI.Events;

public class RepairOrderRejected : Event
{
    public readonly Guid RepairOrderId;
    public readonly DateTime RejectedAt;
    public readonly string RejectReason;

    public RepairOrderRejected(Guid messageId, Guid repairOrderId, string rejectReason, DateTime rejectedAt)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        RejectReason = rejectReason;
        RejectedAt = rejectedAt;
    }
}