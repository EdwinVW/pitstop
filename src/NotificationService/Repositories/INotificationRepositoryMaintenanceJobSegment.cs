using Pitstop.NotificationService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitstop.NotificationService.Repositories
{
    public interface INotificationRepositoryMaintenanceJobSegment
    {
        Task RegisterMaintenanceJobAsync(MaintenanceJob job);
        Task<IEnumerable<MaintenanceJob>> GetMaintenanceJobsForTodayAsync(DateTime date);
        Task RemoveMaintenanceJobsAsync(IEnumerable<string> jobIds);
    }
}
