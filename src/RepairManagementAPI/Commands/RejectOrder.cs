public class RejectOrder
{
    public Guid MessageId { get; set; }
    public Guid RepairOrderId { get; set; }
    public string RejectReason { get; set; }
    public DateTime RejectedAt { get; set; }

    public RejectOrder(Guid messageId, Guid repairOrderId, string rejectReason)
    {
        MessageId = messageId;
        RepairOrderId = repairOrderId;
        RejectReason = rejectReason;
        RejectedAt = DateTime.UtcNow;
    }
}
