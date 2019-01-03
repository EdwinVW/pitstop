using System;
using Pitstop.WorkshopManagementAPI.Domain;

namespace WorkshopManagement.UnitTests.TestdataBuilders
{
    public class MaintenanceJobBuilder
    {
        public Guid Id { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public string Description { get; private set; }
        public Customer Customer { get; private set; }
        public Vehicle Vehicle { get; private set; }

        public MaintenanceJobBuilder()
        {
            SetDefaults();
        }

        public MaintenanceJobBuilder WithId(Guid id)
        {
            Id = id;
            return this;
        }

        public MaintenanceJobBuilder WithStartTime(DateTime startTime)
        {
            StartTime = startTime;
            return this;
        }

        public MaintenanceJobBuilder WithEndTime(DateTime endTime)
        {
            EndTime = endTime;
            return this;
        }        

        public MaintenanceJobBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public MaintenanceJobBuilder WithCustomer(Customer customer)
        {
            Customer = customer;
            return this;
        }

        public MaintenanceJobBuilder WithVehicle(Vehicle vehicle)
        {
            Vehicle = vehicle;
            return this;
        }

        public MaintenanceJob Build()
        {
            if (Customer == null)
            {
                throw new InvalidOperationException("You must specify a customer using the 'WithCustomer' method.");
            }

            if (Vehicle == null)
            {
                throw new InvalidOperationException("You must specify a vehicle using the 'WithVehicle' method.");
            }

            var job = new MaintenanceJob();
            job.Plan(Id, StartTime, EndTime, Vehicle, Customer, Description);
            return job;
        }

        private void SetDefaults()
        {
            Id = Guid.NewGuid();
            StartTime = DateTime.Today.AddHours(8);
            EndTime = DateTime.Today.AddHours(11);
            Description = $"Job {Id}";
        }
    }
}