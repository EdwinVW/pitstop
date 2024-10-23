namespace Pitstop.RepairManagementAPI.Commands;

public class SendRepairOrder : Command
{
    public CustomerInfo CustomerInfo { get; set; }
    public VehicleInfo VehicleInfo { get; set; }

    public List<Guid> ToRepairVehicleParts { get; set; }

    public decimal TotalCost { get; set; }
    public decimal LaborCost { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }

    public SendRepairOrder(
        Guid messageId,
        CustomerInfo customerInfo,
        VehicleInfo vehicleInfo,
        List<Guid> toRepairVehicleParts,
        decimal totalCost,
        decimal laborCost,
        bool isApproved,
        DateTime createdAt,
        string status): base(messageId)
    {
        CustomerInfo = customerInfo;
        VehicleInfo = vehicleInfo;
        ToRepairVehicleParts = toRepairVehicleParts;
        TotalCost = totalCost;
        LaborCost = laborCost;
        IsApproved = isApproved;
        CreatedAt = createdAt;
        Status = status;
    }
}

public class CustomerInfo
{
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
}

public class VehicleInfo
{
    public string LicenseNumber { get; set; }
    public string Type { get; set; }
    public string Brand { get; set; }
}