namespace Pitstop.InvoiceService.Events;

public class CustomerRegistered : Event
{
    public readonly string CustomerId;
    public readonly string Name;
    public readonly string Address;
    public readonly string PostalCode;
    public readonly string City;


    public CustomerRegistered(Guid messageId, string customerId, string name, string address, string postalCode, string city) : base(messageId)
    {
        CustomerId = customerId;
        Name = name;
        Address = address;
        PostalCode = postalCode;
        City = city;
    }
}
