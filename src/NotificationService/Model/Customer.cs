namespace Pitstop.NotificationService.Model;

public class Customer
{
    public string CustomerId { get; set; }
    public string Name { get; set; }
    public string TelephoneNumber { get; set; }
    public string EmailAddress { get; set; }

    public static Customer CreateFrom(CustomerRegistered source) => new Customer
    {
        CustomerId = source.CustomerId,
        Name = source.Name,
        TelephoneNumber = source.TelephoneNumber,
        EmailAddress = source.EmailAddress
    };
}