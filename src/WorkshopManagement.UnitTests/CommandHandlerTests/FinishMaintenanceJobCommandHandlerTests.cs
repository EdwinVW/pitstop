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
    public class FinishMaintenanceJobCommandHandlerTests
    {
        [Fact]
        public async void FinishMaintenanceJob_Handler_Should_Handle_Command()
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
            WorkshopPlanning planning = new WorkshopPlanning(initializingEvents);

            FinishMaintenanceJob command = new FinishMaintenanceJobCommandBuilder()
                .WithJobId(jobId)
                .WithActualStartTime(actualStartTime)
                .WithActualEndTime(actualEndTime)
                .Build();

            Mock<IMessagePublisher> messagePublisherMock = new Mock<IMessagePublisher>();
            Mock<IWorkshopPlanningRepository> repoMock = new Mock<IWorkshopPlanningRepository>();

            repoMock
                .Setup(m => m.GetWorkshopPlanningAsync(It.Is<DateTime>(p => p == date)))
                .Returns(Task.FromResult(planning));

            repoMock
                .Setup(m => m.SaveWorkshopPlanningAsync(
                    It.Is<string>(p => p == planning.Id),
                    It.Is<int>(p => p == 2),
                    It.Is<int>(p => p == 3),
                    It.IsAny<IEnumerable<Event>>()
                ))
                .Returns(Task.CompletedTask);

            messagePublisherMock
                .Setup(m => m.PublishMessageAsync(
                    It.Is<string>(p => p == "MaintenanceJobFinished"),
                    It.IsAny<object>(),
                    ""))
                .Returns(Task.CompletedTask);

            FinishMaintenanceJobCommandHandler sut = 
                new FinishMaintenanceJobCommandHandler(messagePublisherMock.Object, repoMock.Object);

            // act
            await sut.HandleCommandAsync(date, command);
            
            // assert
            messagePublisherMock.VerifyAll();
            messagePublisherMock.VerifyNoOtherCalls();
            repoMock.VerifyAll();            
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void Given_A_Non_Existing_Job_The_Handler_Should_Return_Null()
        {
            // arrange
            DateTime date = DateTime.Today;
            Guid jobId = Guid.NewGuid();
            DateTime actualStartTime = date.AddHours(9);
            DateTime actualEndTime = date.AddHours(12);
            FinishMaintenanceJob command = new FinishMaintenanceJobCommandBuilder()
                .WithJobId(jobId)
                .WithActualStartTime(actualStartTime)
                .WithActualEndTime(actualEndTime)
                .Build();

            Mock<IMessagePublisher> messagePublisherMock = new Mock<IMessagePublisher>();
            Mock<IWorkshopPlanningRepository> repoMock = new Mock<IWorkshopPlanningRepository>();

            repoMock
                .Setup(m => m.GetWorkshopPlanningAsync(It.Is<DateTime>(p => p == date)))
                .Returns(Task.FromResult<WorkshopPlanning>(null));

            FinishMaintenanceJobCommandHandler sut = 
                new FinishMaintenanceJobCommandHandler(messagePublisherMock.Object, repoMock.Object);

            // act
            var result = await sut.HandleCommandAsync(date, command);
            
            // assert
            messagePublisherMock.VerifyAll();
            messagePublisherMock.VerifyNoOtherCalls();
            repoMock.VerifyAll();
            repoMock.VerifyNoOtherCalls();
            Assert.Null(result);
        }         
    }
}