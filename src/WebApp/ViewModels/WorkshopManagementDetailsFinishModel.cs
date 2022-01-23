namespace Pitstop.WebApp.ViewModels;

public class WorkshopManagementFinishViewModel
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    [Display(Name = "Started at")]
    [DataType(DataType.Time)]
    public DateTime? ActualStartTime { get; set; }

    [Display(Name = "Completed at")]
    [DataType(DataType.Time)]
    public DateTime? ActualEndTime { get; set; }

    [Display(Name = "Mechanic notes")]
    public string Notes { get; set; }
}