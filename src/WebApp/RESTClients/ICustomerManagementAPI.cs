namespace WebApp.RESTClients;

public interface ICustomerManagementAPI
{
    [Get("/api/customers")]
    Task<List<Customer>> GetCustomers();

    [Get("/api/customers/{id}")]
    Task<Customer> GetCustomerById([AliasAs("id")] string customerId);

    [Post("/api/customers")]
    Task RegisterCustomer(RegisterCustomer command);
}