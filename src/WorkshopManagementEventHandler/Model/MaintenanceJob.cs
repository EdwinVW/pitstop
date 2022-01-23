namespace Pitstop.WorkshopManagementEventHandler.Model;

public class MaintenanceJob
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Vehicle Vehicle { get; set; }
    public Customer Customer { get; set; }
    public string Description { get; set; }
    public DateTime? ActualStartTime { get; set; }
    public DateTime? ActualEndTime { get; set; }
    public string Notes { get; set; }
    public DateTime WorkshopPlanningDate { get; set; }
}