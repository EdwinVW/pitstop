using System.Linq;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain.Entities;
using Pitstop.WorkshopManagementAPI.Domain.Exceptions;

namespace Pitstop.WorkshopManagementAPI.Domain.BusinessRules
{
    public static class WorkshopPlanningRules
    {
        /// <summary>
        /// The maximum number of parallel jobs in the workshop (restricted by the available workstations).
        /// </summary>
        private const int AVAILABLE_WORKSTATIONS = 3;
        
        public static void PlannedMaintenanceJobShouldFallWithinOneBusinessDay(
            this WorkshopPlanning planning, PlanMaintenanceJob command)
        {
            if (command.StartTime.Date != command.EndTime.Date)
            {
                throw new BusinessRuleViolationException("Start-time and end-time of a Maintenance Job must be within a 1 day.");
            }
        }

        public static void NumberOfParallelMaintenanceJobsMustNotExceedAvailableWorkStations(
            this WorkshopPlanning planning, PlanMaintenanceJob command)
        {
            if (planning.Jobs.Count(j =>
                (j.PlannedTimeslot.StartTime >= command.StartTime && j.PlannedTimeslot.StartTime <= command.EndTime) ||
                (j.PlannedTimeslot.EndTime >= command.StartTime && j.PlannedTimeslot.EndTime <= command.EndTime)) >= AVAILABLE_WORKSTATIONS)
            {
                throw new BusinessRuleViolationException($"Maintenancejob overlaps with more than {AVAILABLE_WORKSTATIONS} other jobs.");
            }
        }

        public static void NumberOfParallelMaintenanceJobsOnAVehicleIsOne(
            this WorkshopPlanning planning, PlanMaintenanceJob command)
        {
            if (planning.Jobs.Any(j => j.Vehicle.Id == command.VehicleInfo.LicenseNumber &&
                    (j.PlannedTimeslot.StartTime >= command.StartTime && j.PlannedTimeslot.StartTime <= command.EndTime ||
                    j.PlannedTimeslot.EndTime >= command.StartTime && j.PlannedTimeslot.EndTime <= command.EndTime)))
            {
                throw new BusinessRuleViolationException($"Only 1 maintenance job can be executed on a vehicle during a certain time-slot.");
            }
        }
    }
}