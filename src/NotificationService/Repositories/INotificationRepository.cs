namespace Pitstop.NotificationService.Repositories
{
    public interface INotificationRepository :
        INotificationRepositoryCustomerSegment,
        INotificationRepositoryMaintenanceJobSegment
    { }
}
