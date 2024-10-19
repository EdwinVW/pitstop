using Pitstop.Infrastructure.Messaging;
using Pitstop.RepairManagementAPI.Commands;
using Pitstop.RepairManagementAPI.Enums;

namespace Pitstop.RepairManagementAPI.Events;

public class RepairOrderApproved : Event
{
    public readonly string RepairOrderId;
    public readonly RepairOrdersStatus Status;
    public readonly DateTime ApprovalOnDate;

    public RepairOrderApproved(Guid messageId, string repairOrderId, RepairOrdersStatus status,
        DateTime approvalDate)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        Status = status;
        ApprovalOnDate = approvalDate;
    }

    public static RepairOrderApproved FromCommand(ApproveRepairOrder command)
    {
        return new RepairOrderApproved(
            Guid.NewGuid(),
            command.RepairOrderId,
            command.Status,
            command.ApprovalOnDate
        );
    }
}