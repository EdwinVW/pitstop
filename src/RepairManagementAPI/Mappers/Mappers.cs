using Pitstop.RepairManagementAPI.Commands;
using Pitstop.RepairManagementAPI.Model;

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
}