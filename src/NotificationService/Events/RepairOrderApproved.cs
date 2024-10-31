namespace Pitstop.NotificationService.Events;
public class RepairOrderApproved : Event
{
    public readonly Guid RepairOrderId;
    public readonly DateTime ApproveDate;
    public readonly string CustomerId;
    public readonly string CustomerName;
    public readonly string LicenseNumber;

    public RepairOrderApproved(Guid messageId, Guid repairOrderId, DateTime approveDate, string customerId, string customerName, string licenseNumber)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        ApproveDate = approveDate;
        CustomerId = customerId;
        customerName = CustomerName;
        licenseNumber = LicenseNumber;
    }
}