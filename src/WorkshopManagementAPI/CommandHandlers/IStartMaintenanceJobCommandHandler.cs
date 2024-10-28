namespace Pitstop.WorkshopManagementAPI.CommandHandlers;

public interface IStartMaintenanceJobCommandHandler
{
    Task<WorkshopPlanning> HandleCommandAsync(DateTime planningDate, StartMaintenanceJob command);
}