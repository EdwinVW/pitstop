using System;
using Pitstop.ViewModels;
using Pitstop.ViewModels;
using WebApp.Commands;

namespace Pitstop.WebApp.Mappers
{
    public static class Mappers
    {
        public static RegisterCustomer MapToRegisterCustomer(this CustomerManagementNewViewModel source) => new RegisterCustomer
        (
            Guid.NewGuid(),
            Guid.NewGuid().ToString("N"),
            source.Customer.Name,
            source.Customer.Address,
            source.Customer.PostalCode,
            source.Customer.City,
            source.Customer.TelephoneNumber,
            source.Customer.EmailAddress
        );

        public static RegisterVehicle MapToRegisterVehicle(this VehicleManagementNewViewModel source) => new RegisterVehicle(
            Guid.NewGuid(),
            source.Vehicle.LicenseNumber,
            source.Vehicle.Brand,
            source.Vehicle.Type,
            source.SelectedCustomerId
        );

        public static UpdateVehicle MapToUpdateVehicle(this VehicleManagementEditViewModel source) => new UpdateVehicle(
            Guid.NewGuid(),
            source.Vehicle.LicenseNumber,
            source.Vehicle.Brand,
            source.Vehicle.Type,
            source.SelectedCustomerId
        );
    }
}