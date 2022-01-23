namespace Pitstop.WorkshopManagementAPI.Domain.BusinessRules;

public static class MaintenanceJobRules
{
    public static void PlannedMaintenanceJobShouldFallWithinOneBusinessDay(this PlanMaintenanceJob command)
    {
        if (!Timeslot.Create(command.StartTime, command.EndTime).IsWithinOneDay())
        {
            throw new BusinessRuleViolationException("Start-time and end-time of a Maintenance Job must be within a 1 day.");
        }
    }

    public static void FinishedMaintenanceJobCanNotBeFinished(this MaintenanceJob job)
    {
        if (job.Status == "Completed")
        {
            throw new BusinessRuleViolationException($"An already finished job can not be finished.");
        }
    }
}