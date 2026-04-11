using Pitstop.TestUtils;

namespace Pitstop.InvoiceService.UnitTests.TestdataBuilders;

/// <summary>
/// Builds a <see cref="Pitstop.InvoiceService.Model.Customer"/>.
/// </summary>
public class CustomerBuilder
{
    public string CustomerId { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public string PostalCode { get; private set; }
    public string City { get; private set; }

    public CustomerBuilder()
    {
        SetDefaults();
    }

    public CustomerBuilder WithCustomerId(string customerId)
    {
        CustomerId = customerId;
        return this;
    }

    public CustomerBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public CustomerBuilder WithAddress(string address)
    {
        Address = address;
        return this;
    }

    public CustomerBuilder WithPostalCode(string postalCode)
    {
        PostalCode = postalCode;
        return this;
    }

    public CustomerBuilder WithCity(string city)
    {
        City = city;
        return this;
    }

    public Pitstop.InvoiceService.Model.Customer Build()
    {
        return new Pitstop.InvoiceService.Model.Customer
        {
            CustomerId = CustomerId,
            Name = Name,
            Address = Address,
            PostalCode = PostalCode,
            City = City
        };
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
