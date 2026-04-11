namespace InvoiceService.UnitTests.InvoiceWorkerTests;

[TestClass]
public class HandleMaintenanceJobPlannedTests
{
    [TestMethod]
    public async Task HandleMessage_With_MaintenanceJobPlanned_Should_Register_Job_In_Repository()
    {
        // arrange
        MaintenanceJobPlannedEventBuilder builder = new MaintenanceJobPlannedEventBuilder();
        Pitstop.InvoiceService.Events.MaintenanceJobPlanned ev = builder.Build();
        string json = JsonConvert.SerializeObject(ev);

        Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
        Mock<IInvoiceRepository> repoMock = new Mock<IInvoiceRepository>();
        Mock<IEmailCommunicator> emailMock = new Mock<IEmailCommunicator>();

        repoMock
            .Setup(r => r.RegisterMaintenanceJobAsync(It.Is<MaintenanceJob>(j =>
                j.JobId == builder.JobId &&
                j.CustomerId == builder.CustomerId &&
                j.LicenseNumber == builder.LicenseNumber &&
                j.Description == builder.Description)))
            .Returns(Task.CompletedTask);

        InvoiceWorker sut = new InvoiceWorker(messageHandlerMock.Object, repoMock.Object, emailMock.Object);

        // act
        await sut.HandleMessageAsync("MaintenanceJobPlanned", json);

        // assert
        repoMock.VerifyAll();
        repoMock.VerifyNoOtherCalls();
        emailMock.VerifyNoOtherCalls();
    }
}
