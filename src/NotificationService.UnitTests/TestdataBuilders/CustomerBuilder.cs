using Pitstop.TestUtils;

namespace Pitstop.NotificationService.UnitTests.TestdataBuilders;

public class CustomerBuilder
{
    public string CustomerId { get; private set; }
    public string Name { get; private set; }
    public string TelephoneNumber { get; private set; }
    public string EmailAddress { get; private set; }

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

    public CustomerBuilder WithTelephoneNumber(string telephoneNumber)
    {
        TelephoneNumber = telephoneNumber;
        return this;
    }

    public CustomerBuilder WithEmailAddress(string emailAddress)
    {
        EmailAddress = emailAddress;
        return this;
    }

    public Pitstop.NotificationService.Model.Customer Build()
    {
        return new Pitstop.NotificationService.Model.Customer
        {
            CustomerId = CustomerId,
            Name = Name,
            TelephoneNumber = TelephoneNumber,
            EmailAddress = EmailAddress
        };
    }

    private void SetDefaults()
    {
        CustomerId = TestDataPrimitives.RandomGuid();
        var name = TestDataPrimitives.RandomName();
        Name = name;
        TelephoneNumber = TestDataPrimitives.RandomPhoneNumber();
        EmailAddress = TestDataPrimitives.RandomEmailAddress(name);
    }
}
