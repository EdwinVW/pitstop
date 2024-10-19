using Pitstop.Infrastructure.Messaging;
using Pitstop.RepairManagementAPI.Enums;

namespace Pitstop.RepairManagementAPI.Commands;

public class ApproveRepairOrder : Command
{
    public readonly string RepairOrderId;
    public readonly RepairOrdersStatus Status;
    public readonly DateTime ApprovalOnDate;

    public ApproveRepairOrder(Guid messageId, string repairOrderId, RepairOrdersStatus status, DateTime updatedAt)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        Status = status;
        ApprovalOnDate = updatedAt;
    }
}