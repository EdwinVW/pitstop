namespace NotificationService.UnitTests.NotificationWorkerTests;

[TestClass]
public class HandleMaintenanceJobFinishedTests
{
    [TestMethod]
    public async Task HandleMessage_With_MaintenanceJobFinished_Should_Remove_Job_From_Repository()
    {
        // arrange
        MaintenanceJobFinishedEventBuilder builder = new MaintenanceJobFinishedEventBuilder();
        MaintenanceJobFinished ev = builder.Build();
        string json = JsonConvert.SerializeObject(ev);

        Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
        Mock<INotificationRepository> repoMock = new Mock<INotificationRepository>();
        Mock<IEmailNotifier> emailMock = new Mock<IEmailNotifier>();

        repoMock
            .Setup(r => r.RemoveMaintenanceJobsAsync(
                It.Is<IEnumerable<string>>(ids => ids.Contains(builder.JobId))))
            .Returns(Task.CompletedTask);

        NotificationWorker sut = new NotificationWorker(messageHandlerMock.Object, repoMock.Object, emailMock.Object);

        // act
        await sut.HandleMessageAsync("MaintenanceJobFinished", json);

        // assert
        repoMock.VerifyAll();
        repoMock.VerifyNoOtherCalls();
        emailMock.VerifyNoOtherCalls();
    }
}
