using Pitstop.Infrastructure.Messaging;
using Pitstop.RepairManagementAPI.Commands;
using Pitstop.RepairManagementAPI.Enums;
using Pitstop.RepairManagementAPI.Model;

namespace Pitstop.RepairManagementAPI.Events;

public class RepairOrderCreated : Event
{
    public readonly string RepairOrderId;
    public readonly string CustomerId;
    public string LicenseNumber { get; set; }
    public readonly List<VehicleParts> VehicleParts;
    public readonly decimal TotalCost;
    public readonly decimal LaborCost;
    public readonly bool IsApproved;
    public readonly DateTime CreatedAt;
    public readonly RepairOrdersStatus Status;

    public RepairOrderCreated(
        Guid messageId,
        string repairOrderId,
        string customerId,
        string licenseNumber,
        List<VehicleParts> vehicleParts,
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
        VehicleParts = vehicleParts;
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
            command.VehicleParts,
            command.TotalCost,
            command.LaborCost,
            command.IsApproved,
            command.CreatedAt,
            command.Status
        );
    }
}