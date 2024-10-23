namespace Pitstop.RepairManagementAPI.Commands;

public class ApproveRepairOrder : Command
{
    public readonly Guid RepairOrderId;
    public readonly DateTime ApproveDate;

    public ApproveRepairOrder(Guid messageId, Guid repairOrderId, DateTime approveDate)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        ApproveDate = approveDate;
    }
}