namespace Pitstop.CustomerManagementAPI.Mappers;

public static class Mappers
{
    public static CustomerRegistered MapToCustomerRegistered(this RegisterCustomer command) => new CustomerRegistered
    (
        System.Guid.NewGuid(),
        command.CustomerId,
        command.Name,
        command.Address,
        command.PostalCode,
        command.City,
        command.TelephoneNumber,
        command.EmailAddress
    );

    public static Customer MapToCustomer(this RegisterCustomer command) => new Customer
    {
        CustomerId = command.CustomerId,
        Name = command.Name,
        Address = command.Address,
        PostalCode = command.PostalCode,
        City = command.City,
        TelephoneNumber = command.TelephoneNumber,
        EmailAddress = command.EmailAddress
    };
}