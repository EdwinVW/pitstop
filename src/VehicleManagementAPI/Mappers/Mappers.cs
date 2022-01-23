namespace Pitstop.VehicleManagementAPI.Mappers;

public static class Mappers
{
    public static Vehicle MapToVehicle(this RegisterVehicle command) => new Vehicle
    {
        LicenseNumber = command.LicenseNumber,
        Brand = command.Brand,
        Type = command.Type,
        OwnerId = command.OwnerId
    };
}