namespace WebApp.RESTClients;

public interface ICustomerSupportAPI
{
    [Get("/customersupport")]
    Task<List<Rejection>> GetRejections();
}