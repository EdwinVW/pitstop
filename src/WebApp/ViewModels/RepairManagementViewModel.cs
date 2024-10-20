namespace Pitstop.WebApp.ViewModels;

public class RepairManagementViewModel
{
    public List<CustomerVehicleViewModel> CustomerVehicles { get; set; } // Combinatie van klanten en voertuigen

    public class CustomerVehicleViewModel
    {
        public string CustomerName { get; set; }
        public string LicenseNumber { get; set; }
        public string RepairOrderStatus { get; set; } // Status van de Repair Order, of null als er geen is
    }
}