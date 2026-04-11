using Pitstop.TestUtils;

namespace Pitstop.NotificationService.UnitTests.TestdataBuilders;

public class CustomerRegisteredEventBuilder
{
    public string CustomerId { get; private set; }
    public string Name { get; private set; }
    public string TelephoneNumber { get; private set; }
    public string EmailAddress { get; private set; }

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

    public CustomerRegisteredEventBuilder WithTelephoneNumber(string telephoneNumber)
    {
        TelephoneNumber = telephoneNumber;
        return this;
    }

    public CustomerRegisteredEventBuilder WithEmailAddress(string emailAddress)
    {
        EmailAddress = emailAddress;
        return this;
    }

    public Pitstop.NotificationService.Events.CustomerRegistered Build()
    {
        return new Pitstop.NotificationService.Events.CustomerRegistered(
            Guid.NewGuid(), CustomerId, Name, TelephoneNumber, EmailAddress);
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
