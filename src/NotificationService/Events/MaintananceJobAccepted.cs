namespace Pitstop.NotificationService.Events;

public class MaintenanceJobAccepted : Event
{
    public readonly Guid repairOrderId;
    public readonly string MechanicEmail;
    public readonly Guid CustomerId;

    public MaintenanceJobAccepted(Guid messageId, Guid RepairOrderId, string mechanicEmail, Guid customerId) :
        base(messageId)
    {
        repairOrderId = RepairOrderId;
        MechanicEmail = mechanicEmail;
        CustomerId = customerId;
    }
}
