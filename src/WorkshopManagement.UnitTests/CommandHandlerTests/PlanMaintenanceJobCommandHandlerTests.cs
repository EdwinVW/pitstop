using System;
using System.Collections.Generic;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI.Commands;
using WorkshopManagement.UnitTests.TestdataBuilders;
using WorkshopManagementAPI.CommandHandlers;
using Xunit;
using Moq;
using Pitstop.WorkshopManagementAPI.Repositories;
using System.Threading.Tasks;
using System.Linq;
using Pitstop.WorkshopManagementAPI.Domain.Entities;

namespace WorkshopManagement.UnitTests.CommandHandlerTests
{
    [Collection("AutomapperCollection")]
    public class PlanMaintenanceJobCommandHandlerTests
    {
        [Fact]
        public async void Given_An_Existing_Job_The_Handler_Should_Handle_The_Command()
        {
            // arrange
            DateTime date = DateTime.Today;
            var initializingEvents = new Event[] { 
                new WorkshopPlanningCreatedEventBuilder().WithDate(date).Build() 
            };
            WorkshopPlanning planning = new WorkshopPlanning(date, initializingEvents);

            PlanMaintenanceJob command = new PlanMaintenanceJobCommandBuilder()
                .Build();

            Mock<IMessagePublisher> messagePublisherMock = new Mock<IMessagePublisher>();
            Mock<IWorkshopPlanningRepository> repoMock = new Mock<IWorkshopPlanningRepository>();

            repoMock
                .Setup(m => m.GetWorkshopPlanningAsync(It.Is<DateTime>(p => p == date)))
                .Returns(Task.FromResult(planning));

            repoMock
                .Setup(m => m.SaveWorkshopPlanningAsync(
                    It.Is<string>(p => p == planning.Id),
                    It.Is<int>(p => p == 1),
                    It.Is<int>(p => p == 2),
                    It.IsAny<List<Event>>()
                ))
                .Returns(Task.CompletedTask);

            messagePublisherMock
                .Setup(m => m.PublishMessageAsync(
                    It.Is<string>(p => p == "MaintenanceJobPlanned"),
                    It.IsAny<object>(),
                    ""))
                .Returns(Task.CompletedTask);

            PlanMaintenanceJobCommandHandler sut = 
                new PlanMaintenanceJobCommandHandler(messagePublisherMock.Object, repoMock.Object);

            // act
            WorkshopPlanning result = await sut.HandleCommandAsync(date, command);
            
            // assert
            messagePublisherMock.VerifyAll();
            messagePublisherMock.VerifyNoOtherCalls();
            repoMock.VerifyAll();
            repoMock.VerifyNoOtherCalls();
            Assert.IsAssignableFrom<WorkshopPlanning>(result);
        }        

        [Fact]
        public async void Given_A_Non_Existing_Job_The_Handler_Should_Return_Null()
        {
            // arrange
            DateTime date = DateTime.Today;
            string workshopPlanningId = date.ToString("yyy-MM-dd");
            PlanMaintenanceJob command = new PlanMaintenanceJobCommandBuilder()
                .Build();

            Mock<IMessagePublisher> messagePublisherMock = new Mock<IMessagePublisher>();
            Mock<IWorkshopPlanningRepository> repoMock = new Mock<IWorkshopPlanningRepository>();

            repoMock
                .Setup(m => m.GetWorkshopPlanningAsync(It.Is<DateTime>(p => p == date)))
                .Returns(Task.FromResult<WorkshopPlanning>(null));

            repoMock
                .Setup(m => m.SaveWorkshopPlanningAsync(
                    It.Is<string>(p => p == workshopPlanningId),
                    It.Is<int>(p => p == 0),
                    It.Is<int>(p => p == 2),
                    It.Is<IEnumerable<Event>>(p =>  
                        p.First().MessageType == "WorkshopPlanningCreated" && 
                        p.Last().MessageType == "MaintenanceJobPlanned")
                ))
                .Returns(Task.CompletedTask);

            messagePublisherMock
                .Setup(m => m.PublishMessageAsync(
                    It.Is<string>(p => p == "WorkshopPlanningCreated"),
                    It.IsAny<object>(),
                    ""))
                .Returns(Task.CompletedTask);

            messagePublisherMock
                .Setup(m => m.PublishMessageAsync(
                    It.Is<string>(p => p == "MaintenanceJobPlanned"),
                    It.IsAny<object>(),
                    ""))
                .Returns(Task.CompletedTask);

            PlanMaintenanceJobCommandHandler sut = 
                new PlanMaintenanceJobCommandHandler(messagePublisherMock.Object, repoMock.Object);

            // act
            WorkshopPlanning result = await sut.HandleCommandAsync(date, command);
            
            // assert
            messagePublisherMock.VerifyAll();
            messagePublisherMock.VerifyNoOtherCalls();
            repoMock.VerifyAll();
            repoMock.VerifyNoOtherCalls();
            Assert.IsAssignableFrom<WorkshopPlanning>(result);        }            
    }
}