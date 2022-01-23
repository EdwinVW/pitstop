namespace Pitstop.NotificationService.Repositories;

public interface INotificationRepository
{
    Task RegisterCustomerAsync(Customer customer);
    Task RegisterMaintenanceJobAsync(MaintenanceJob job);
    Task<IEnumerable<MaintenanceJob>> GetMaintenanceJobsForTodayAsync(DateTime date);
    Task<Customer> GetCustomerAsync(string customerId);
    Task RemoveMaintenanceJobsAsync(IEnumerable<string> jobIds);
}