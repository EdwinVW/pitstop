namespace Pitstop.WebApp.Commands;

public class RegisterPlanning : Command
{
    public readonly DateTime PlanningDate;

    public RegisterPlanning(Guid messageId, DateTime planningDate) : base(messageId)
    {
        PlanningDate = planningDate;
    }
}