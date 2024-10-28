namespace Pitstop.WebApp.ViewModels;

public class RepairManagementNewViewModel
{
    public RepairOrder RepairOrder { get; set; }
    public List<VehicleParts> AvailableVehicleParts { get; set; }

    public List<Guid> SelectedVehicleParts { get; set; }
}