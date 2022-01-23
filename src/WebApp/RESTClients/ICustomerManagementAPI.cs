namespace WebApp.RESTClients;

public interface ICustomerManagementAPI
{
    [Get("/customers")]
    Task<List<Customer>> GetCustomers();

    [Get("/customers/{id}")]
    Task<Customer> GetCustomerById([AliasAs("id")] string customerId);

    [Post("/customers")]
    Task RegisterCustomer(RegisterCustomer command);
}