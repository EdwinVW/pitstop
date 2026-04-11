namespace NotificationService.UnitTests.NotificationWorkerTests;

[TestClass]
public class HandleMaintenanceJobPlannedTests
{
    [TestMethod]
    public async Task HandleMessage_With_MaintenanceJobPlanned_Should_Register_Job_In_Repository()
    {
        // arrange
        MaintenanceJobPlannedEventBuilder builder = new MaintenanceJobPlannedEventBuilder();
        MaintenanceJobPlanned ev = builder.Build();
        string json = JsonConvert.SerializeObject(ev);

        Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
        Mock<INotificationRepository> repoMock = new Mock<INotificationRepository>();
        Mock<IEmailNotifier> emailMock = new Mock<IEmailNotifier>();

        repoMock
            .Setup(r => r.RegisterMaintenanceJobAsync(It.Is<MaintenanceJob>(j =>
                j.JobId == builder.JobId.ToString() &&
                j.CustomerId == builder.CustomerId &&
                j.LicenseNumber == builder.LicenseNumber &&
                j.Description == builder.Description)))
            .Returns(Task.CompletedTask);

        NotificationWorker sut = new NotificationWorker(messageHandlerMock.Object, repoMock.Object, emailMock.Object);

        // act
        await sut.HandleMessageAsync("MaintenanceJobPlanned", json);

        // assert
        repoMock.VerifyAll();
        repoMock.VerifyNoOtherCalls();
        emailMock.VerifyNoOtherCalls();
    }
}
