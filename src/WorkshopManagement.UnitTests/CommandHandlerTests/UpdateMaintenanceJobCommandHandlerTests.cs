using Moq;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI.Domain;
using Pitstop.WorkshopManagementAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopManagement.UnitTests.TestdataBuilders;
using WorkshopManagementAPI.CommandHandlers;
using Xunit;

namespace WorkshopManagement.UnitTests.CommandHandlerTests
{
    public class UpdateMaintenanceJobCommandHandlerTests
    {
        [Fact]
        public async void UpdateMaintenanceJob_Handler_Should_Handle_Command()
        {
            var date = DateTime.Today;
            var jobId = Guid.NewGuid();
            var startTime = date.AddHours(8);
            var endTime = date.AddHours(11);
            var description = "Job has been updated";

            var createdEvent = new WorkshopPlanningCreatedEventBuilder()
                    .WithDate(date)
                    .Build();

            var plannedEvent = new MaintenanceJobPlannedEventBuilder()
                    .WithStartTime(startTime)
                    .WithEndTime(endTime)
                    .WithJobId(jobId)
                    .Build();

            var initializingEvents = new Event[] {
                createdEvent,
                plannedEvent
            };

            var planning = new WorkshopPlanning(date, initializingEvents);

            var command = new UpdateMaintenanceJobCommandBuilder()
                .WithEvent(plannedEvent)
                .WithChangedDescription(description)
                .Build();

            var messagePublisherMock = new Mock<IMessagePublisher>();
            var repositoryMock = new Mock<IWorkshopPlanningRepository>();

            repositoryMock
                .Setup(m => m.GetWorkshopPlanningAsync(It.Is<DateTime>(p => p == date)))
                .Returns(Task.FromResult(planning));

            repositoryMock
                .Setup(m => m.SaveWorkshopPlanningAsync(
                    It.Is<string>(p => p == planning.Id),
                    It.Is<int>(p => p == 2),
                    It.Is<int>(p => p == 3),
                    It.IsAny<List<Event>>()
                ))
                .Returns(Task.CompletedTask);

            messagePublisherMock
                .Setup(m => m.PublishMessageAsync(
                    It.Is<string>(p => p == "MaintenanceJobUpdated"),
                    It.IsAny<object>(),
                    ""))
                .Returns(Task.CompletedTask);

            var sut = new UpdateMaintenanceJobCommandHandler(messagePublisherMock.Object, repositoryMock.Object);

            // act
            var result = await sut.HandleCommandAsync(date, command);

            // assert
            messagePublisherMock.VerifyAll();
            messagePublisherMock.VerifyNoOtherCalls();
            repositoryMock.VerifyAll();
            repositoryMock.VerifyNoOtherCalls();
            Assert.IsAssignableFrom<WorkshopPlanning>(result);
            Assert.NotEmpty(result.Jobs.Where(j => j.Description == description));
        }

        [Fact]
        public async Task Given_A_Non_Existing_Command_Handler_Should_Return_NullAsync()
        {

            // arrange
            var date = DateTime.Today;
            var jobId = Guid.NewGuid();
            var actualStartTime = date.AddHours(9);
            var actualEndTime = date.AddHours(12);
            var description = "Job has been updated";

            var command = new UpdateMaintenanceJobCommandBuilder()
                .WithChangedDescription(description)
                .Build();

            var messagePublisherMock = new Mock<IMessagePublisher>();
            var repoMock = new Mock<IWorkshopPlanningRepository>();

            repoMock
                .Setup(m => m.GetWorkshopPlanningAsync(It.Is<DateTime>(p => p == date)))
                .Returns(Task.FromResult<WorkshopPlanning>(null));

            var sut = new UpdateMaintenanceJobCommandHandler(messagePublisherMock.Object, repoMock.Object);

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
