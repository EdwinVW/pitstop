namespace Pitstop.RepairManagementAPI.Mappers;

public static class Mappers
{
    public static RepairOrders MapToRepairOrders(this CreateRepairOrder command) => new RepairOrders
    {
        Id = command.RepairOrderId,
        CustomerId = command.CustomerId,
        LicenseNumber = command.LicenseNumber,
        VehicleParts = command.VehicleParts,
        IsApproved = command.IsApproved,
        TotalCost = command.TotalCost,
        CreatedAt = command.CreatedAt,
        Status = command.Status,
    };

    public static RepairOrderCreated MapToRepairOrderCreated(CreateRepairOrder command)
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

    public static RepairOrderApproved MapToRepairOrderApproved(ApproveRepairOrder command)
    {
        return new RepairOrderApproved(
            Guid.NewGuid(),
            command.RepairOrderId
        );
    }

    public static RepairOrderRejected MapToRepairOrderRejected(RejectRepairOrder command)
    {
        return new RepairOrderRejected(
            Guid.NewGuid(),
            command.RepairOrderId,
            command.RejectReason
        );
    }
}