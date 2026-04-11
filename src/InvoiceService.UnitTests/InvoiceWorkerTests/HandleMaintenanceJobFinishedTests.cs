namespace InvoiceService.UnitTests.InvoiceWorkerTests;

[TestClass]
public class HandleMaintenanceJobFinishedTests
{
    [TestMethod]
    public async Task HandleMessage_With_MaintenanceJobFinished_Should_Mark_Job_As_Finished_In_Repository()
    {
        // arrange
        MaintenanceJobFinishedEventBuilder builder = new MaintenanceJobFinishedEventBuilder();
        Pitstop.InvoiceService.Events.MaintenanceJobFinished ev = builder.Build();
        string json = JsonConvert.SerializeObject(ev);

        Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
        Mock<IInvoiceRepository> repoMock = new Mock<IInvoiceRepository>();
        Mock<IEmailCommunicator> emailMock = new Mock<IEmailCommunicator>();

        repoMock
            .Setup(r => r.MarkMaintenanceJobAsFinished(
                It.Is<string>(id => id == builder.JobId),
                It.Is<DateTime>(t => t == builder.StartTime),
                It.Is<DateTime>(t => t == builder.EndTime)))
            .Returns(Task.CompletedTask);

        InvoiceWorker sut = new InvoiceWorker(messageHandlerMock.Object, repoMock.Object, emailMock.Object);

        // act
        await sut.HandleMessageAsync("MaintenanceJobFinished", json);

        // assert
        repoMock.VerifyAll();
        repoMock.VerifyNoOtherCalls();
        emailMock.VerifyNoOtherCalls();
    }
}
