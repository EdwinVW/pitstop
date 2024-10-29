public class RejectRepairOrder
{
    public Guid MessageId { get; set; }
    public Guid RepairOrderId { get; set; }
    public string RejectReason { get; set; }
    public DateTime RejectedAt { get; set; }

    public RejectRepairOrder(Guid messageId, Guid repairOrderId, string rejectReason)
    {
        MessageId = messageId;
        RepairOrderId = repairOrderId;
        RejectReason = rejectReason;
    }
}
