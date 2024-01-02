namespace WebApp.RESTClients;

public interface IVehicleManagementAPI
{
    [Get("/api/vehicles")]
    Task<List<Vehicle>> GetVehicles();

    [Get("/api/vehicles/{id}")]
    Task<Vehicle> GetVehicleByLicenseNumber([AliasAs("id")] string licenseNumber);

    [Post("/api/vehicles")]
    Task RegisterVehicle(RegisterVehicle command);
}