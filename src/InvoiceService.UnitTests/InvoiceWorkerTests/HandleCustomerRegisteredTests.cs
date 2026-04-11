namespace InvoiceService.UnitTests.InvoiceWorkerTests;

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
        Mock<IInvoiceRepository> repoMock = new Mock<IInvoiceRepository>();
        Mock<IEmailCommunicator> emailMock = new Mock<IEmailCommunicator>();

        repoMock
            .Setup(r => r.RegisterCustomerAsync(It.Is<Customer>(c =>
                c.CustomerId == builder.CustomerId &&
                c.Name == builder.Name &&
                c.Address == builder.Address &&
                c.PostalCode == builder.PostalCode &&
                c.City == builder.City)))
            .Returns(Task.CompletedTask);

        InvoiceWorker sut = new InvoiceWorker(messageHandlerMock.Object, repoMock.Object, emailMock.Object);

        // act
        await sut.HandleMessageAsync("CustomerRegistered", json);

        // assert
        repoMock.VerifyAll();
        repoMock.VerifyNoOtherCalls();
        emailMock.VerifyNoOtherCalls();
    }
}
