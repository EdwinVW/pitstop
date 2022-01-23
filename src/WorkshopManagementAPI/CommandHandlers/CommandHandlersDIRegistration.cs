namespace Pitstop.WorkshopManagementAPI.CommandHandlers;

public static class CommandHandlersDIRegistration
{
    public static void AddCommandHandlers(this IServiceCollection services)
    {
        services.AddTransient<IPlanMaintenanceJobCommandHandler, PlanMaintenanceJobCommandHandler>();
        services.AddTransient<IFinishMaintenanceJobCommandHandler, FinishMaintenanceJobCommandHandler>();
    }
}