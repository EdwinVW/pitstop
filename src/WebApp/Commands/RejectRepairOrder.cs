namespace Pitstop.WebApp.Commands;

public class RejectRepairOrder : Command
{
    public readonly string RejectReason;

    public RejectRepairOrder(Guid messageId, string repairOrderId, string rejectReason)
        : base(messageId)
    {
        RejectReason = rejectReason;
    }
}