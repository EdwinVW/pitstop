namespace Pitstop.Infrastructure.Messaging
{
    /// <summary>
    /// Complete list of message types in the system.
    /// </summary>
    public enum MessageTypes
    {
        // General
        Unknown, 

        // Commands
        RegisterCustomer,
        RegisterVehicle,
        PlanMaintenanceJob,
        FinishMaintenanceJob,

        // Events
        DayHasPassed,
        CustomerRegistered,
        VehicleRegistered,
        WorkshopPlanningCreated,
        MaintenanceJobPlanned,
        MaintenanceJobFinished
    }
}
