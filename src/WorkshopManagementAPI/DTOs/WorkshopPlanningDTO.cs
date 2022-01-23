namespace Pitstop.WorkshopManagementAPI.DTOs;

public class WorkshopPlanningDTO
{
    public DateTime Date { get; set; }
    public List<MaintenanceJobDTO> Jobs { get; set; }
}