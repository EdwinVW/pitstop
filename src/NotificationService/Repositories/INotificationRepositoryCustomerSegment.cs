using Pitstop.NotificationService.Model;
using System.Threading.Tasks;

namespace Pitstop.NotificationService.Repositories
{
    public interface INotificationRepositoryCustomerSegment
    {
        Task<Customer> GetCustomerAsync(string customerId);
        Task RegisterCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
    }
}
