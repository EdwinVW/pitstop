namespace Pitstop.RepairManagementAPI.Commands;

public class ApproveOrder : Command
{
    public readonly Guid RepairOrderId;
    public readonly DateTime ApproveDate;
    public readonly string CustomerId;

    public ApproveOrder(Guid messageId, Guid repairOrderId, DateTime approveDate, string customerId)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        ApproveDate = approveDate;
        CustomerId = customerId;
    }
}