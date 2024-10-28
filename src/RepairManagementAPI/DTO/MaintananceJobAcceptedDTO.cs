namespace Pitstop.NotificationService
{
    public class MaintenanceJobAcceptedDto
    {
        public Guid JobId { get; set; }
        public string MechanicEmail { get; set; }
        public Guid CustomerId { get; set; }

    }
}
