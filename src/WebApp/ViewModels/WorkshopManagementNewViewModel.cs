namespace Pitstop.WebApp.ViewModels;

public class WorkshopManagementNewViewModel
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    [Required]
    [DataType(DataType.Time)]
    [Display(Name = "Start Time")]
    public DateTime StartTime { get; set; }

    [Required]
    [Display(Name = "End Time")]
    [DataType(DataType.Time)]
    public DateTime EndTime { get; set; }

    [Required]
    [Display(Name = "Description")]
    public string Description { get; set; }

    public IEnumerable<SelectListItem> Vehicles { get; set; }

    [Required(ErrorMessage = "Vehicle is required")]
    [Display(Name = "Vehicle")]
    public string SelectedVehicleLicenseNumber { get; set; }

    public string Error { get; set; }
}