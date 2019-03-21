using System;
using Pitstop.WorkshopManagementAPI.Events;

namespace WorkshopManagement.UnitTests.TestdataBuilders
{
    public class MaintenanceJobPlannedEventBuilder
    {
        public MaintenanceJobBuilder MaintenanceJobBuilder { get; private set; }
        public CustomerBuilder CustomerBuilder { get; private set; }
        public VehicleBuilder VehicleBuilder { get; private set; }

        public MaintenanceJobPlannedEventBuilder()
        {
            SetDefaults();
        }

        public MaintenanceJobPlannedEventBuilder WithJobId(Guid jobId)
        {
            MaintenanceJobBuilder.WithJobId(jobId);
            return this;
        }        

        public MaintenanceJobPlannedEventBuilder WithStartTime(DateTime startTime)
        {
            MaintenanceJobBuilder.WithStartTime(startTime);
            return this;
        } 

        public MaintenanceJobPlannedEventBuilder WithEndTime(DateTime endTime)
        {
            MaintenanceJobBuilder.WithEndTime(endTime);
            return this;
        }         

        public MaintenanceJobPlanned Build()
        {
             var customer = CustomerBuilder
                .Build();
            
            var vehicle = VehicleBuilder
                .WithOwnerId(customer.CustomerId)
                .Build();
            
            var job = MaintenanceJobBuilder
                .WithCustomer(customer)
                .WithVehicle(vehicle)
                .Build();

            MaintenanceJobPlanned e = new MaintenanceJobPlanned(
                Guid.NewGuid(), job.Id, job.StartTime, job.EndTime,
                (customer.CustomerId, customer.Name, customer.TelephoneNumber),
                (vehicle.LicenseNumber, vehicle.Brand, vehicle.Type),
                job.Description
            );

            return e;
        }

        private void SetDefaults()
        {
            CustomerBuilder = new CustomerBuilder();
            VehicleBuilder = new VehicleBuilder();
            MaintenanceJobBuilder = new MaintenanceJobBuilder();
        }
    }
}