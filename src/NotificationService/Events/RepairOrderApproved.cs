namespace Pitstop.NotificationService.Events;
public class RepairOrderApproved : Event
{
    public readonly Guid RepairOrderId;
    public readonly DateTime ApproveDate;

    public readonly string CustomerId;

    public RepairOrderApproved(Guid messageId, Guid repairOrderId, DateTime approveDate, string customerId)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        ApproveDate = approveDate;
        CustomerId = customerId;
    }
}