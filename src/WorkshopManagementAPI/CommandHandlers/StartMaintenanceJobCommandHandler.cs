namespace Pitstop.WorkshopManagementAPI.CommandHandlers;

public class StartMaintenanceJobCommandHandler : IStartMaintenanceJobCommandHandler
{
    IMessagePublisher _messagePublisher;
    IEventSourceRepository<WorkshopPlanning> _planningRepo;

    public StartMaintenanceJobCommandHandler(IMessagePublisher messagePublisher, IEventSourceRepository<WorkshopPlanning> planningRepo)
    {
        _messagePublisher = messagePublisher;
        _planningRepo = planningRepo;
    }

    public async Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, StartMaintenanceJob command)
    {
        // get planning
        var aggregateId = WorkshopPlanningId.Create(planningDate);
        var planning = await _planningRepo.GetByIdAsync(aggregateId);
        if (planning == null)
        {
            return null;
        }

        // handle command
        planning.StartMaintenanceJob(command);

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