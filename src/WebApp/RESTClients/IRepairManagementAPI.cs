public interface IRepairManagementAPI
{
    // Get all repair orders
    [Get("/repairManagement/repairorders")]
    Task<List<RepairOrders>> GetAllRepairOrders();

    // Get a specific repair order by ID
    [Get("/repairManagement/repairorders/{id}")]
    Task<RepairOrders> GetRepairOrderById(Guid id);

    // Create a new repair order
    [Post("/repairManagement/repairorders/create")]
    Task CreateRepairOrder([Body] CreateRepairOrder command);

    // Approve a repair order
    [Post("/repairManagement/approve/{repairOrderId}")]
    Task ApproveRepairOrder([AliasAs("repairOrderId")] Guid repairOrderId, [Body] ApproveRepairOrder command);

    // Reject a repair order
    [Post("/repairManagement/reject/{repairOrderId}")]
    Task RejectRepairOrder([AliasAs("repairOrderId")] Guid repairOrderId, [Body] RejectRepairOrder command);

    // Get all vehicle parts
    [Get("/repairManagement/vehicleparts")]
    Task<List<VehicleParts>> GetVehicleParts();
}