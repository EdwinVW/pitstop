namespace Pitstop.WebApp.Commands;

public class SendRepairOrder : Command
{
    public readonly CustomerInfo CustomerInfo;
    public readonly VehicleInfo VehicleInfo;
    public readonly List<Guid> ToRepairVehicleParts;
    public readonly decimal TotalCost;
    public readonly decimal LaborCost;
    public readonly bool IsApproved;
    public readonly DateTime CreatedAt;
    public readonly string Status;

    public SendRepairOrder(
        Guid messageId,
        CustomerInfo customerInfo,
        VehicleInfo vehicleInfo,
        List<Guid> toRepairVehicleParts,
        decimal totalCost,
        decimal laborCost,
        bool isApproved,
        DateTime createdAt,
        string status)
        : base(messageId)
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