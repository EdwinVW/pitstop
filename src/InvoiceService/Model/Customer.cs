namespace Pitstop.InvoiceService.Model;

public class Customer
{
    public string CustomerId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }

    public static Customer CreateFrom(CustomerRegistered source) => new Customer
    {
        CustomerId = source.CustomerId,
        Name = source.Name,
        Address = source.Address,
        PostalCode = source.PostalCode,
        City = source.City
    };
}

