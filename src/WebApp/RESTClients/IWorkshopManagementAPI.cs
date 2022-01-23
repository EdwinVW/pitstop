namespace WebApp.RESTClients;

public interface IWorkshopManagementAPI
{
    [Get("/workshopplanning/{planningDate}")]
    Task<WorkshopPlanning> GetWorkshopPlanning(string planningDate);

    [Get("/workshopplanning/{planningDate}/jobs/{jobId}")]
    Task<MaintenanceJob> GetMaintenanceJob(string planningDate, string jobId);

    [Post("/workshopplanning/{planningDate}")]
    Task RegisterPlanning(string planningDate, RegisterPlanning cmd);

    [Post("/workshopplanning/{planningDate}/jobs")]
    Task PlanMaintenanceJob(string planningDate, PlanMaintenanceJob cmd);

    [Put("/workshopplanning/{planningDate}/jobs/{jobId}/finish")]
    Task FinishMaintenanceJob(string planningDate, string jobId, FinishMaintenanceJob cmd);

    [Get("/refdata/customers")]
    Task<List<Customer>> GetCustomers();

    [Get("/refdata/customers/{id}")]
    Task<Customer> GetCustomerById(string id);

    [Get("/refdata/vehicles")]
    Task<List<Vehicle>> GetVehicles();

    [Get("/refdata/vehicles/{id}")]
    Task<Vehicle> GetVehicleByLicenseNumber([AliasAs("id")] string licenseNumber);
}