public interface IRepairManagementAPI
{
    // Get all repair orders
    [Get("/repairManagement")]
    Task<List<RepairOrder>> GetAllRepairOrders();

    // Get repair order by ID
    [Get("/repairManagement/{repairOrderId}")]
    Task<RepairOrder> GetRepairOrderById(string repairOrderId);

    // Send a new repair order
    [Post("/repairManagement/send")]
    Task SendRepairOrder([Body] SendRepairOrder command);

    // Approve a repair order by ID
    [Post("/repairManagement/approve/{repairOrderId}")]
    Task ApproveRepairOrder(string repairOrderId, [Body] ApproveRepairOrder command);
    // Reject a repair order by ID
    [Post("/repairManagement/reject/{repairOrderId}")]
    Task RejectRepairOrder(string repairOrderId, [Body] RejectRepairOrder command);
    // Get all vehicle parts
    [Get("/repairManagement/vehicleparts")]
    Task<List<VehicleParts>> GetVehicleParts();
}