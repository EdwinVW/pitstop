using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Events;
using System;

namespace WorkshopManagement.UnitTests.TestdataBuilders
{
    public class UpdateMaintenanceJobCommandBuilder
    {
        public PlanMaintenanceJob PlanMaintenanceJob { get; private set; }
        public Guid Id { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public (string Id, string Name, string TelephoneNumber) CustomerInfo { get; private set; }
        public (string LicenseNumber, string Brand, string Type) VehicleInfo { get; private set; }
        public string Description { get; private set; }

        public UpdateMaintenanceJobCommandBuilder()
        {
            SetDefaults();
        }

        public UpdateMaintenanceJobCommandBuilder WithCommand(PlanMaintenanceJob job)
        {
            PlanMaintenanceJob = job;
            return this;
        }

        public UpdateMaintenanceJobCommandBuilder WithEvent(MaintenanceJobPlanned plannedEvent)
        {
            Id = plannedEvent.JobId;
            StartTime = plannedEvent.StartTime;
            EndTime = plannedEvent.EndTime;
            CustomerInfo = plannedEvent.CustomerInfo;
            VehicleInfo = plannedEvent.VehicleInfo;
            Description = plannedEvent.Description;

            return this;
        }

        public UpdateMaintenanceJobCommandBuilder WithChangedStartTime(DateTime startTime)
        {
            StartTime = startTime;
            return this;
        }

        public UpdateMaintenanceJobCommandBuilder WithChangedEndTime(DateTime endTime)
        {
            EndTime = endTime;
            return this;
        }


        public UpdateMaintenanceJobCommandBuilder WithChangedDescription(string description)
        {
            Description = description;
            return this;
        }

        public UpdateMaintenanceJobCommandBuilder WithChangedCustomerInfo((string Id, string Name, string TelephoneNumber) customerInfo)
        {
            CustomerInfo = customerInfo;
            return this;
        }

        public UpdateMaintenanceJobCommandBuilder WithChangedVehicleInfo((string LicenseNumber, string Brand, string Type) vehicleInfo)
        {
            VehicleInfo = vehicleInfo;
            return this;
        }

        public UpdateMaintenanceJob Build()
        {
            var command = new UpdateMaintenanceJob(
                Guid.NewGuid(),
                Id,
                StartTime,
                EndTime,
                CustomerInfo,
                VehicleInfo,
                Description
            );

            return command;
        }

        private void SetDefaults()
        {
            PlanMaintenanceJob = new PlanMaintenanceJobCommandBuilder()
                .Build();

            Id = PlanMaintenanceJob.JobId;
            StartTime = PlanMaintenanceJob.StartTime;
            EndTime = PlanMaintenanceJob.EndTime;
            CustomerInfo = PlanMaintenanceJob.CustomerInfo;
            VehicleInfo = PlanMaintenanceJob.VehicleInfo;
            Description = PlanMaintenanceJob.Description;
        }
    }
}
