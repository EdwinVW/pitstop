namespace Pitstop.WebApp.ViewModels;

public class RepairManagementNewViewModel
{
    public string CustomerId { get; set; }

    [Required] public string CustomerName { get; set; }

    [Required] public string LicenseNumber { get; set; }

    public decimal TotalCost { get; set; }

    public decimal LaborCost { get; set; }
    
    public List<VehicleParts> AvailableVehicleParts { get; set; }
    
    public List<string> SelectedVehicleParts { get; set; }
}

