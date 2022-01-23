namespace Pitstop.WorkshopManagementAPI.Repositories;

using Pitstop.WorkshopManagementAPI.Repositories.Model;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> GetVehiclesAsync();
    Task<Vehicle> GetVehicleAsync(string licenseNumber);
}