namespace Pitstop.NotificationService.Events;

public class MaintenanceJobRejected : Event
{
    public readonly string repairOrderId;
    public readonly string MechanicEmail;
    public readonly string CustomerId;

    public MaintenanceJobRejected(Guid messageId, string RepairOrderId, string mechanicEmail, string customerId) :
        base(messageId)
    {
        repairOrderId = RepairOrderId;
        MechanicEmail = mechanicEmail;
        CustomerId = customerId;
    }
}