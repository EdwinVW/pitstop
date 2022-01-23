namespace Pitstop.WorkshopManagementAPI.CommandHandlers;

public interface IPlanMaintenanceJobCommandHandler
{
    Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, PlanMaintenanceJob command);
}