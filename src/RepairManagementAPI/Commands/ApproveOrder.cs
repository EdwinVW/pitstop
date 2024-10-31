namespace Pitstop.RepairManagementAPI.Commands;

public class ApproveOrder : Command
{
    public readonly Guid RepairOrderId;
    public readonly DateTime ApproveDate;
    public readonly string CustomerId;
    public readonly string CustomerName;
    public readonly string LicenseNumber;

    public ApproveOrder(Guid messageId, Guid repairOrderId, DateTime approveDate, string customerId, string customerName, string licenseNumber)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        ApproveDate = approveDate;
        CustomerId = customerId;
        CustomerName = customerName;
        LicenseNumber = licenseNumber;
    }
}