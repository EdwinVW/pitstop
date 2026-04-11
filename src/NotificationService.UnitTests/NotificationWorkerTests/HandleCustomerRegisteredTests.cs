namespace NotificationService.UnitTests.NotificationWorkerTests;

[TestClass]
public class HandleCustomerRegisteredTests
{
    [TestMethod]
    public async Task HandleMessage_With_CustomerRegistered_Should_Register_Customer_In_Repository()
    {
        // arrange
        CustomerRegisteredEventBuilder builder = new CustomerRegisteredEventBuilder();
        CustomerRegistered ev = builder.Build();
        string json = JsonConvert.SerializeObject(ev);

        Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
        Mock<INotificationRepository> repoMock = new Mock<INotificationRepository>();
        Mock<IEmailNotifier> emailMock = new Mock<IEmailNotifier>();

        repoMock
            .Setup(r => r.RegisterCustomerAsync(It.Is<Customer>(c =>
                c.CustomerId == builder.CustomerId &&
                c.Name == builder.Name &&
                c.TelephoneNumber == builder.TelephoneNumber &&
                c.EmailAddress == builder.EmailAddress)))
            .Returns(Task.CompletedTask);

        NotificationWorker sut = new NotificationWorker(messageHandlerMock.Object, repoMock.Object, emailMock.Object);

        // act
        await sut.HandleMessageAsync("CustomerRegistered", json);

        // assert
        repoMock.VerifyAll();
        repoMock.VerifyNoOtherCalls();
        emailMock.VerifyNoOtherCalls();
    }
}
