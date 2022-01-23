namespace Pitstop.WorkshopManagementAPI.CommandHandlers;

public class FinishMaintenanceJobCommandHandler : IFinishMaintenanceJobCommandHandler
{
    IMessagePublisher _messagePublisher;
    IEventSourceRepository<WorkshopPlanning> _planningRepo;

    public FinishMaintenanceJobCommandHandler(IMessagePublisher messagePublisher, IEventSourceRepository<WorkshopPlanning> planningRepo)
    {
        _messagePublisher = messagePublisher;
        _planningRepo = planningRepo;
    }

    public async Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, FinishMaintenanceJob command)
    {
        // get planning
        var aggregateId = WorkshopPlanningId.Create(planningDate);
        var planning = await _planningRepo.GetByIdAsync(aggregateId);
        if (planning == null)
        {
            return null;
        }

        // handle command
        planning.FinishMaintenanceJob(command);

        // persist
        IEnumerable<Event> events = planning.GetEvents();
        await _planningRepo.SaveAsync(
            planning.Id, planning.OriginalVersion, planning.Version, events);

        // publish event
        foreach (var e in events)
        {
            await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
        }

        // return result
        return planning;
    }
}