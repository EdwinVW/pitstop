namespace WebApp.RESTClients;

public interface IVehicleManagementAPI
{
    [Get("/vehicles")]
    Task<List<Vehicle>> GetVehicles();

    [Get("/vehicles/{id}")]
    Task<Vehicle> GetVehicleByLicenseNumber([AliasAs("id")] string licenseNumber);

    [Post("/vehicles")]
    Task RegisterVehicle(RegisterVehicle command);
}