namespace WebApp.RESTClients;

public interface IWorkshopManagementAPI
{
    [Get("/api/workshopplanning/{planningDate}")]
    Task<WorkshopPlanning> GetWorkshopPlanning(string planningDate);

    [Get("/api/workshopplanning/{planningDate}/jobs/{jobId}")]
    Task<MaintenanceJob> GetMaintenanceJob(string planningDate, string jobId);

    [Post("/api/workshopplanning/{planningDate}")]
    Task RegisterPlanning(string planningDate, RegisterPlanning cmd);

    [Post("/api/workshopplanning/{planningDate}/jobs")]
    Task PlanMaintenanceJob(string planningDate, PlanMaintenanceJob cmd);

    [Put("/api/workshopplanning/{planningDate}/jobs/{jobId}/finish")]
    Task FinishMaintenanceJob(string planningDate, string jobId, FinishMaintenanceJob cmd);

    [Get("/api/refdata/customers")]
    Task<List<Customer>> GetCustomers();

    [Get("/api/refdata/customers/{id}")]
    Task<Customer> GetCustomerById(string id);

    [Get("/api/refdata/vehicles")]
    Task<List<Vehicle>> GetVehicles();

    [Get("/api/refdata/vehicles/{id}")]
    Task<Vehicle> GetVehicleByLicenseNumber([AliasAs("id")] string licenseNumber);
}