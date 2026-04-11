namespace InvoiceService.UnitTests.ModelTests;

[TestClass]
public class CustomerTests
{
    [TestMethod]
    public void CreateFrom_Should_Map_All_Fields_From_CustomerRegistered_Event()
    {
        // arrange
        CustomerRegisteredEventBuilder builder = new CustomerRegisteredEventBuilder();
        CustomerRegistered ev = builder.Build();

        // act
        Customer sut = Customer.CreateFrom(ev);

        // assert
        Assert.AreEqual(builder.CustomerId, sut.CustomerId);
        Assert.AreEqual(builder.Name, sut.Name);
        Assert.AreEqual(builder.Address, sut.Address);
        Assert.AreEqual(builder.PostalCode, sut.PostalCode);
        Assert.AreEqual(builder.City, sut.City);
    }
}
