namespace Pitstop.WorkshopManagementAPI.Domain.BusinessRules;

public static class WorkshopPlanningRules
{
    /// <summary>
    /// The maximum number of parallel jobs in the workshop (restricted by the available workstations).
    /// </summary>
    private const int AVAILABLE_WORKSTATIONS = 3;

    public static void NumberOfParallelMaintenanceJobsMustNotExceedAvailableWorkStations(
        this WorkshopPlanning planning, PlanMaintenanceJob command)
    {
        if (planning.Jobs.Count(j => j.PlannedTimeslot.OverlapsWith(command.StartTime, command.EndTime)) >= AVAILABLE_WORKSTATIONS)
        {
            throw new BusinessRuleViolationException($"Maintenancejob overlaps with more than {AVAILABLE_WORKSTATIONS} other jobs.");
        }
    }

    public static void NumberOfParallelMaintenanceJobsOnAVehicleMustNotExceedOne(
        this WorkshopPlanning planning, PlanMaintenanceJob command)
    {
        if (planning.Jobs.Any(j => j.Vehicle.Id == command.VehicleInfo.LicenseNumber &&
                j.PlannedTimeslot.OverlapsWith(command.StartTime, command.EndTime)))
        {
            throw new BusinessRuleViolationException($"Only 1 maintenance job can be executed on a vehicle during a certain time-slot.");
        }
    }
}