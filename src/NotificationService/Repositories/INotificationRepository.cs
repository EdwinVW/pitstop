using Pitstop.NotificationService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitstop.NotificationService.Repositories
{
    public interface INotificationRepository
    {
        Task RegisterCustomerAsync(Customer customer);
        Task RegisterMaintenanceJobAsync(MaintenanceJob job);
        Task<IEnumerable<MaintenanceJob>> GetMaintenanceJobsForTodayAsync(DateTime date);
        Task<Customer> GetCustomerAsync(string customerId);
        Task RemoveMaintenanceJobsAsync(IEnumerable<string> jobIds);
    }
}
