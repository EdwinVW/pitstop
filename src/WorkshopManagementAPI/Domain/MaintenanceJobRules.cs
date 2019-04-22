using Pitstop.WorkshopManagementAPI.Domain.Exceptions;

namespace Pitstop.WorkshopManagementAPI.Domain
{
    public static class MaintenanceJobRules
    {
        public static void FinishedMaintenanceJobCanNotBeFinished(this MaintenanceJob job)
        {
            if (job.Status == "Completed")
            {
                throw new BusinessRuleViolationException($"An already finished job can not be finished.");
            }
        }
    }
}