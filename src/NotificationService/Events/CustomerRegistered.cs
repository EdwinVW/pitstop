namespace Pitstop.NotificationService.Events;

public class CustomerRegistered : Event
{
    public readonly string CustomerId;
    public readonly string Name;
    public readonly string TelephoneNumber;
    public readonly string EmailAddress;

    public CustomerRegistered(Guid messageId, string customerId, string name, string telephoneNumber, string emailAddress) :
        base(messageId)
    {
        CustomerId = customerId;
        Name = name;
        TelephoneNumber = telephoneNumber;
        EmailAddress = emailAddress;
    }
}