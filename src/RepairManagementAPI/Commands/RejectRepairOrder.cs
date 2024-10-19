using Pitstop.Infrastructure.Messaging;
using Pitstop.RepairManagementAPI.Enums;

namespace Pitstop.RepairManagementAPI.Commands;

public class RejectRepairOrder : Command
{
    public readonly string RepairOrderId;
    public readonly RepairOrdersStatus Status;
    public readonly DateTime RejectionDate;
    public readonly string RejectReason;

    public RejectRepairOrder(Guid messageId, string repairOrderId, RepairOrdersStatus status, string rejectReason,
        DateTime rejectionDate)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        Status = status;
        RejectReason = rejectReason;
        RejectionDate = rejectionDate;
    }
}