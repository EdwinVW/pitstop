namespace Pitstop.WebApp.Commands;

public class ApproveRepairOrder : Command
{
    public ApproveRepairOrder(Guid messageId)
        : base(messageId)
    {
    }
}