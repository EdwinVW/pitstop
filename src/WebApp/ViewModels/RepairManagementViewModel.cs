namespace Pitstop.WebApp.ViewModels;

public class RepairManagementViewModel
{
    public List<RepairManagementCustomerVehicleViewModel> RepairOrders { get; set; }

    public class RepairManagementCustomerVehicleViewModel
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string LicenseNumber { get; set; }
        public string RepairOrderStatus { get; set; }
    }
}