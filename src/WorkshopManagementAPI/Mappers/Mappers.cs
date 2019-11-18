using System;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Events;

namespace Pitstop.WorkshopManagementAPI.Mappers
{
    public static class Mappers
    {
        public static MaintenanceJobPlanned MapToMaintenanceJobPlanned(this PlanMaintenanceJob source) => new MaintenanceJobPlanned(
            Guid.NewGuid(),
            source.JobId,
            source.StartTime,
            source.EndTime,
            source.CustomerInfo,
            source.VehicleInfo,
            source.Description
        );

        public static MaintenanceJobUpdated MapToMaintenanceJobUpdated(this UpdateMaintenanceJob source) => new MaintenanceJobUpdated(
            Guid.NewGuid(),
            source.JobId,
            source.StartTime,
            source.EndTime,
            source.CustomerInfo,
            source.VehicleInfo,
            source.Description
        );

        public static MaintenanceJobFinished MapToMaintenanceJobFinished(this FinishMaintenanceJob source) => new MaintenanceJobFinished
        (
            Guid.NewGuid(),
            source.JobId,
            source.StartTime,
            source.EndTime,
            source.Notes
        );
    }
}