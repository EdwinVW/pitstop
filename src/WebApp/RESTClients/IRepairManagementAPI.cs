namespace Pitstop.WebApp.RESTClients;

public interface IRepairManagementAPI
{
    [Get("/repairManagement")]
    Task<List<RepairOrders>> GetAllRepairOrders();

    [Get("/repairManagement/{id}")]
    Task<RepairOrders> GetRepairOrderById(Guid id);

    [Post("/repairManagement/create")]
    Task CreateRepairOrder([Body] CreateRepairOrder command);


    [Post("/repairManagement/approve/{repairOrderId}")]
    Task ApproveRepairOrder([AliasAs("repairOrderId")] int repairOrderId, [Body] ApproveRepairOrder command);

    [Post("/repairManagement/{repairOrderId}/reject")]
    Task RejectRepairOrder([AliasAs("repairOrderId")] int repairOrderId, [Body] RejectRepairOrder command);
}