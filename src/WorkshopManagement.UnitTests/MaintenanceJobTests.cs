using System;
using Xunit;
using Pitstop.WorkshopManagementAPI.Domain;
using WorkshopManagement.UnitTests.TestdataBuilders;

namespace WorkshopManagement.UnitTests
{
    public class MaintenanceJobTests
    {
        [Fact]
        public void Plan_Should_Create_A_New_Job()
        {
            // arrange
            CustomerBuilder customerBuilder = new CustomerBuilder();
            VehicleBuilder vehicleBuilder = new VehicleBuilder();
            MaintenanceJobBuilder maintenanceJobBuilder = new MaintenanceJobBuilder();
            Customer customer = customerBuilder
                .Build();
            Vehicle vehicle = vehicleBuilder
                .WithCustomerId(customer.CustomerId)
                .Build();
            MaintenanceJob sut = maintenanceJobBuilder
                .WithCustomer(customer)
                .WithVehicle(vehicle)
                .Build();

            // act
            // sut.Plan() is called by the Testdata Builder

            // assert
            Assert.Equal(maintenanceJobBuilder.Id, sut.Id);
            Assert.Equal(maintenanceJobBuilder.StartTime, sut.StartTime);
            Assert.Equal(maintenanceJobBuilder.EndTime, sut.EndTime);
            Assert.Equal(customer, sut.Customer);
            Assert.Equal(vehicle, sut.Vehicle);
            Assert.Equal(maintenanceJobBuilder.Description, sut.Description);
            Assert.Null(sut.ActualStartTime);
            Assert.Null(sut.ActualEndTime);
            Assert.Null(sut.Notes);
            Assert.Equal("Planned", sut.Status);
        }

        [Fact]
        public void Finish_Should_Finish_An_Existing_Job()
        {
            // arrange
            CustomerBuilder customerBuilder = new CustomerBuilder();
            VehicleBuilder vehicleBuilder = new VehicleBuilder();
            MaintenanceJobBuilder maintenanceJobBuilder = new MaintenanceJobBuilder();
            Customer customer = customerBuilder
                .Build();
            Vehicle vehicle = vehicleBuilder
                .WithCustomerId(customer.CustomerId)
                .Build();
            MaintenanceJob sut = maintenanceJobBuilder
                .WithCustomer(customer)
                .WithVehicle(vehicle)
                .Build();

            DateTime actualStartTime = maintenanceJobBuilder.StartTime.AddMinutes(30);
            DateTime actualEndTime = maintenanceJobBuilder.EndTime.AddMinutes(15);
            string notes = $"Mechanic notes {maintenanceJobBuilder.Id}";

            // act
            sut.Finish(actualStartTime, actualEndTime, notes);

            // assert
            Assert.Equal(maintenanceJobBuilder.Id, sut.Id);
            Assert.Equal(maintenanceJobBuilder.StartTime, sut.StartTime);
            Assert.Equal(maintenanceJobBuilder.EndTime, sut.EndTime);
            Assert.Equal(customer, sut.Customer);
            Assert.Equal(vehicle, sut.Vehicle);
            Assert.Equal(maintenanceJobBuilder.Description, sut.Description);
            Assert.Equal(actualStartTime, sut.ActualStartTime.Value);
            Assert.Equal(actualEndTime, sut.ActualEndTime.Value);
            Assert.Equal(notes, sut.Notes);
            Assert.Equal("Completed", sut.Status);
        }
    }
}
