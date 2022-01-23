namespace Pitstop.WorkshopManagementAPI.DTOs;

public class MaintenanceJobDTO
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public VehicleDTO Vehicle { get; set; }
    public CustomerDTO Customer { get; set; }
    public string Description { get; set; }
    public DateTime? ActualStartTime { get; set; }
    public DateTime? ActualEndTime { get; set; }
    public string Notes { get; set; }
    public string Status => (!ActualStartTime.HasValue && !ActualEndTime.HasValue) ? "Planned" : "Completed";
}