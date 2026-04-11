using Pitstop.TestUtils;

namespace Pitstop.InvoiceService.UnitTests.TestdataBuilders;

public class CustomerRegisteredEventBuilder
{
    public string CustomerId { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public string PostalCode { get; private set; }
    public string City { get; private set; }

    public CustomerRegisteredEventBuilder()
    {
        SetDefaults();
    }

    public CustomerRegisteredEventBuilder WithCustomerId(string customerId)
    {
        CustomerId = customerId;
        return this;
    }

    public CustomerRegisteredEventBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public CustomerRegisteredEventBuilder WithAddress(string address)
    {
        Address = address;
        return this;
    }

    public CustomerRegisteredEventBuilder WithPostalCode(string postalCode)
    {
        PostalCode = postalCode;
        return this;
    }

    public CustomerRegisteredEventBuilder WithCity(string city)
    {
        City = city;
        return this;
    }

    public Pitstop.InvoiceService.Events.CustomerRegistered Build()
    {
        return new Pitstop.InvoiceService.Events.CustomerRegistered(
            Guid.NewGuid(), CustomerId, Name, Address, PostalCode, City);
    }

    private void SetDefaults()
    {
        CustomerId = TestDataPrimitives.RandomGuid();
        Name = TestDataPrimitives.RandomName();
        Address = TestDataPrimitives.RandomAddress();
        PostalCode = TestDataPrimitives.RandomPostalCode();
        City = TestDataPrimitives.RandomCity();
    }
}
