namespace Pitstop.WorkshopManagementAPI.Repositories;

using Pitstop.WorkshopManagementAPI.Repositories.Model;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<Customer> GetCustomerAsync(string customerId);
}