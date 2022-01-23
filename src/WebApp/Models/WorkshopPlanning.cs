namespace Pitstop.WebApp.Models;

public class WorkshopPlanning
{
    public DateTime Date { get; set; }
    public List<MaintenanceJob> Jobs { get; set; }
}