namespace Pitstop.WorkshopManagementAPI.CommandHandlers;

public interface IFinishMaintenanceJobCommandHandler
{
    Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, FinishMaintenanceJob command);
}