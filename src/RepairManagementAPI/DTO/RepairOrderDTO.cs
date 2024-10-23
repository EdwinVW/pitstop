namespace Pitstop.RepairManagementAPI.DTO;

public class RepairOrderDTO
{
    public Guid Id { get; set; }

    public CustomerInfo CustomerInfo { get; set; }

    public VehicleInfo VehicleInfo { get; set; }

    public string Status { get; set; }
}

public class CustomerInfo
{
    public string CustomerName { get; set; }
}

public class VehicleInfo
{
    public string LicenseNumber { get; set; }
}