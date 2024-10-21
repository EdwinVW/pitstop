namespace Pitstop.RepairManagementAPI.Events;

public class RepairOrderCreated : Event
{
    public readonly Guid RepairOrderId;
    public readonly string CustomerId;
    public string LicenseNumber { get; set; }
    public readonly List<string> VehiclePartId;
    public readonly decimal TotalCost;
    public readonly decimal LaborCost;
    public readonly bool IsApproved;
    public readonly DateTime CreatedAt;
    public readonly RepairOrdersStatus Status;

    public RepairOrderCreated(
        Guid messageId,
        Guid repairOrderId,
        string customerId,
        string licenseNumber,
        List<string> vehiclePartId,
        decimal totalCost,
        decimal laborCost,
        bool isApproved,
        DateTime createdAt,
        RepairOrdersStatus status
    )
        : base(messageId)
    {
        RepairOrderId = repairOrderId;
        CustomerId = customerId;
        LicenseNumber = licenseNumber;
        VehiclePartId = vehiclePartId;
        TotalCost = totalCost;
        LaborCost = laborCost;
        IsApproved = isApproved;
        CreatedAt = createdAt;
        Status = status;
    }

    public static RepairOrderCreated FromCommand(CreateRepairOrder command)
    {
        return new RepairOrderCreated(
            command.MessageId,
            command.RepairOrderId,
            command.CustomerId,
            command.LicenseNumber,
            command.VehiclePartId,
            command.TotalCost,
            command.LaborCost,
            command.IsApproved,
            command.CreatedAt,
            command.Status
        );
    }
}