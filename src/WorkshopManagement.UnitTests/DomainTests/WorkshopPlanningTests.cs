using System;
using System.Collections.Generic;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain;
using Pitstop.WorkshopManagementAPI.Domain.Exceptions;
using Pitstop.WorkshopManagementAPI.Events;
using WorkshopManagement.UnitTests.TestdataBuilders;
using Xunit;

namespace WorkshopManagement.UnitTests.DomainTests
{
    [Collection("AutomapperCollection")]
    public class WorkshopPlanningTests
    {
        [Fact]
        public void Create_Should_Create_A_New_Instance()
        {
            // arrange
            DateTime date = DateTime.Today;

            // act
            List<Event> events = new List<Event>(WorkshopPlanning.Create(date, out WorkshopPlanning sut));

            // assert
            Assert.NotNull(sut);
            Assert.NotNull(sut.Id);
            Assert.Equal(date, sut.Date);
            Assert.Equal(0, sut.OriginalVersion);
            Assert.Equal(1, sut.Version);
            Assert.Empty(sut.Jobs);
            Assert.Collection(events, item0 => Assert.IsAssignableFrom<WorkshopPlanningCreated>(item0));
        }

        [Fact]
        public void Plan_MaintenanceJob_Should_Add_A_New_MaintenanceJob()
        {
            // arrange
            DateTime date = DateTime.Today;
            var initializingEvents = new Event[] { 
                new WorkshopPlanningCreatedEventBuilder().WithDate(date).Build() 
            };
            WorkshopPlanning sut = new WorkshopPlanning(initializingEvents);

            PlanMaintenanceJob command = new PlanMaintenanceJobCommandBuilder()
                .Build();

            // act
            List<Event> events = new List<Event>(sut.PlanMaintenanceJob(command));

            // assert
            Assert.NotNull(sut);
            Assert.NotNull(sut.Id);
            Assert.Equal(date, sut.Date);
            Assert.Equal(1, sut.OriginalVersion);
            Assert.Equal(2, sut.Version);
            Assert.Collection(sut.Jobs, 
                item0 => { 
                    Assert.Equal(command.JobId, item0.Id);
                    Assert.Equal(command.StartTime, item0.StartTime);
                    Assert.Equal(command.EndTime, item0.EndTime);
                    Assert.Equal(command.CustomerInfo.Id, item0.Customer.CustomerId);
                    Assert.Equal(command.CustomerInfo.Name, item0.Customer.Name);
                    Assert.Equal(command.CustomerInfo.TelephoneNumber, item0.Customer.TelephoneNumber);
                    Assert.Equal(command.VehicleInfo.LicenseNumber, item0.Vehicle.LicenseNumber);
                    Assert.Equal(command.VehicleInfo.Brand, item0.Vehicle.Brand);
                    Assert.Equal(command.VehicleInfo.Type, item0.Vehicle.Type);
                    Assert.Equal(command.CustomerInfo.Id, item0.Vehicle.OwnerId);
                    Assert.Equal(command.Description, item0.Description);
                }
            );
            Assert.Collection(events, item0 => Assert.IsAssignableFrom<MaintenanceJobPlanned>(item0));
        }

        [Fact]
        public void Plan_MaintenanceJob_That_Spans_Two_Days_Should_Throw_Exception()
        {
            // arrange
            var initializingEvents = new Event[] { 
                new WorkshopPlanningCreatedEventBuilder().Build() 
            };
            WorkshopPlanning sut = new WorkshopPlanning(initializingEvents);

            MaintenanceJobBuilder maintenanceJobBuilder = new MaintenanceJobBuilder();
            maintenanceJobBuilder
                .WithStartTime(DateTime.Today.AddHours(-2)); // to make the job span 2 days
            PlanMaintenanceJob command = new PlanMaintenanceJobCommandBuilder()
                .WithMaintenanceJobBuilder(maintenanceJobBuilder)
                .Build();

            // act
            var thrownException = 
                Assert.Throws<BusinessRuleViolationException>(() => sut.PlanMaintenanceJob(command));

            // assert
            Assert.Equal("Start-time and end-time of a Maintenance Job must be within a 1 day.", 
                thrownException.Message);
        }

        [Fact]
        public void Planning_Too_Much_MaintenanceJobs_In_Parallel_Should_Throw_Exception()
        {
            // arrange
            var initializingEvents = new Event[] { 
                new WorkshopPlanningCreatedEventBuilder().WithDate(DateTime.Today).Build() 
            };
            WorkshopPlanning sut = new WorkshopPlanning(initializingEvents);

            VehicleBuilder vehicleBuilder = new VehicleBuilder();
            PlanMaintenanceJobCommandBuilder commandBuilder = new PlanMaintenanceJobCommandBuilder()
                .WithVehicleBuilder(vehicleBuilder);
              
            PlanMaintenanceJob command1 = commandBuilder.Build();
            vehicleBuilder.WithLicenseNumber(Guid.NewGuid().ToString());    
            PlanMaintenanceJob command2 = commandBuilder.Build();
            vehicleBuilder.WithLicenseNumber(Guid.NewGuid().ToString());    
            PlanMaintenanceJob command3 = commandBuilder.Build();
            vehicleBuilder.WithLicenseNumber(Guid.NewGuid().ToString());    
            PlanMaintenanceJob command4 = commandBuilder.Build();

            // act
            sut.PlanMaintenanceJob(command1);
            sut.PlanMaintenanceJob(command2);
            sut.PlanMaintenanceJob(command3);
            var thrownException = Assert.Throws<BusinessRuleViolationException>(() => {
                sut.PlanMaintenanceJob(command4); // 4th parallel job
            });

            // assert
            Assert.Equal("Maintenancejob overlaps with more than 3 other jobs.", 
                thrownException.Message);
        }        

        [Fact]
        public void Plan_Two_MaintenanceJobs_In_Parallel_For_The_Same_Vehicle_Should_Throw_Exception()
        {
            // arrange
            var initializingEvents = new Event[] { 
                new WorkshopPlanningCreatedEventBuilder().Build() 
            };
            WorkshopPlanning sut = new WorkshopPlanning(initializingEvents);
            PlanMaintenanceJob command = new PlanMaintenanceJobCommandBuilder()
                .Build();

            // act
            sut.PlanMaintenanceJob(command);
            var thrownException = Assert.Throws<BusinessRuleViolationException>(() => {
                sut.PlanMaintenanceJob(command); // parallel job for same vehicle
            });

            // assert
            Assert.Equal("Only 1 maintenance job can be executed on a vehicle during a certain time-slot.", 
                thrownException.Message);
        }        

        [Fact]
        public void Finish_MaintenanceJob_Should_Finish_A_New_MaintenanceJob()
        {
            // arrange
            DateTime date = DateTime.Today;
            Guid jobId = Guid.NewGuid();
            DateTime startTime = date.AddHours(8);
            DateTime endTime = date.AddHours(11);
            DateTime actualStartTime = date.AddHours(9);
            DateTime actualEndTime = date.AddHours(12);
            var initializingEvents = new Event[] { 
                new WorkshopPlanningCreatedEventBuilder()
                    .WithDate(date)
                    .Build(), 
                new MaintenanceJobPlannedEventBuilder()
                    .WithStartTime(startTime)
                    .WithEndTime(endTime)
                    .WithJobId(jobId)
                    .Build()
            };
            WorkshopPlanning sut = new WorkshopPlanning(initializingEvents);
            
            FinishMaintenanceJob command = new FinishMaintenanceJobCommandBuilder()
                .WithJobId(jobId)
                .WithActualStartTime(actualStartTime)
                .WithActualEndTime(actualEndTime)
                .Build();

            // act
            List<Event> events = new List<Event>(sut.FinishMaintenanceJob(command));

            // assert
            Assert.NotNull(sut);
            Assert.NotNull(sut.Id);
            Assert.Equal(date, sut.Date);
            Assert.Equal(2, sut.OriginalVersion);
            Assert.Equal(3, sut.Version);
            Assert.Collection(sut.Jobs, 
                item0 => { 
                    Assert.Equal(command.JobId, item0.Id);
                    Assert.Equal(startTime, item0.StartTime);
                    Assert.Equal(endTime, item0.EndTime);
                    Assert.Equal(command.StartTime, item0.ActualStartTime);
                    Assert.Equal(command.Notes, item0.Notes);
                }
            );
            Assert.Collection(events, item0 => Assert.IsAssignableFrom<MaintenanceJobFinished>(item0));
        }        
    }
}
