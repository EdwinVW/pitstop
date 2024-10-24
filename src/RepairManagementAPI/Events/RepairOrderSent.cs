namespace Pitstop.RepairManagementAPI.Events
{
    public class RepairOrderSent : Event
    {
        public readonly Guid RepairOrderId;
        public readonly CustomerInfo CustomerInfo;
        public readonly VehicleInfo VehicleInfo;
        public readonly List<(string Name, decimal Cost)> ToRepairVehicleParts;
        public readonly decimal TotalCost;
        public readonly decimal LaborCost;
        public readonly bool IsApproved;
        public readonly DateTime CreatedAt;
        public readonly string Status;

        public RepairOrderSent(
            Guid messageId,
            Guid repairOrderId,
            CustomerInfo customerInfo,
            VehicleInfo vehicleInfo,
            List<(string Name, decimal Cost)> toRepairVehicleParts,
            decimal totalCost,
            decimal laborCost,
            bool isApproved,
            DateTime createdAt,
            string status
        )
            : base(messageId)
        {
            RepairOrderId = repairOrderId;
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
}