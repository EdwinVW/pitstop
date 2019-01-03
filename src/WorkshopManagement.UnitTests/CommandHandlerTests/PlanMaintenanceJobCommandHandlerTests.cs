using System;
using System.Collections.Generic;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain;
using Pitstop.WorkshopManagementAPI.Domain.Exceptions;
using Pitstop.WorkshopManagementAPI.Events;
using WorkshopManagement.UnitTests.TestdataBuilders;
using WorkshopManagementAPI.CommandHandlers;
using Xunit;
using Moq;
using Pitstop.WorkshopManagementAPI.Repositories;
using System.Threading.Tasks;

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
            WorkshopPlanning planning = new WorkshopPlanning(initializingEvents);

            PlanMaintenanceJob command = new PlanMaintenanceJobCommandBuilder()
                .Build();

            Mock<IMessagePublisher> messagePublisherMock = new Mock<IMessagePublisher>(MockBehavior.Strict);
            Mock<IWorkshopPlanningRepository> repoMock = new Mock<IWorkshopPlanningRepository>(MockBehavior.Strict);

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
            repoMock.VerifyAll();
            Assert.IsAssignableFrom<WorkshopPlanning>(result);
        }        

        [Fact]
        public async void Given_A_Non_Existing_Job_The_Handler_Should_Return_Null()
        {
            // arrange
            DateTime date = DateTime.Today;
            PlanMaintenanceJob command = new PlanMaintenanceJobCommandBuilder()
                .Build();

            Mock<IMessagePublisher> messagePublisherMock = new Mock<IMessagePublisher>(MockBehavior.Strict);
            Mock<IWorkshopPlanningRepository> repoMock = new Mock<IWorkshopPlanningRepository>(MockBehavior.Strict);

            repoMock
                .Setup(m => m.GetWorkshopPlanningAsync(It.Is<DateTime>(p => p == date)))
                .Returns(Task.FromResult<WorkshopPlanning>(null));

            repoMock
                .Setup(m => m.SaveWorkshopPlanningAsync(
                    It.Is<string>(p => p == date.ToString("yyyy-MM-dd")),
                    It.Is<int>(p => p == 0),
                    It.Is<int>(p => p == 2),
                    It.Is<List<Event>>(p =>  
                        p[0].MessageType == "WorkshopPlanningCreated" && 
                        p[1].MessageType == "MaintenanceJobPlanned")
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
            repoMock.VerifyAll();
            Assert.IsAssignableFrom<WorkshopPlanning>(result);        }            
    }
}