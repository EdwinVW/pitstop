namespace Pitstop.WebApp.Commands;

public class RejectRepairOrder : Command
{
    public readonly string RejectReason;

    public RejectRepairOrder(Guid messageId, string rejectReason, string rejectReason1)
        : base(messageId)
    {
        RejectReason = rejectReason;
    }
}