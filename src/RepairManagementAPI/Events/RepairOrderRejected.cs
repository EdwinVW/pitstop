using Pitstop.Infrastructure.Messaging;
using Pitstop.RepairManagementAPI.Commands;
using Pitstop.RepairManagementAPI.Enums;

namespace Pitstop.RepairManagementAPI.Events;

public class RepairOrderRejected : Event
{
    public readonly string RepairOrderId;
    public readonly RepairOrdersStatus Status;
    public readonly DateTime RejectionDate;
    public readonly string RejectReason;

    public RepairOrderRejected(Guid messageId, string repairOrderId, RepairOrdersStatus status, string rejectReason,
        DateTime rejectionDate)
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        Status = status;
        RejectReason = rejectReason;
        RejectionDate = rejectionDate;
    }

    public static RepairOrderRejected FromCommand(RejectRepairOrder command)
    {
        return new RepairOrderRejected(
            command.MessageId,
            command.RepairOrderId,
            command.Status,
            command.RejectReason,
            command.RejectionDate
        );
    }
}