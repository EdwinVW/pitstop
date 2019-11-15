using Pitstop.Application.VehicleManagement.Commands;
using Pitstop.Application.VehicleManagement.Events;
using Pitstop.Application.VehicleManagement.Model;
using System;

namespace Pitstop.VehicleManagementAPI.Mappers
{
    public static class Mappers 
    { 

        public static Vehicle MapToVehicle(this RegisterVehicle command) => new Vehicle
        {
            LicenseNumber = command.LicenseNumber,
            Brand = command.Brand,
            Type = command.Type,
            OwnerId = command.OwnerId
        };

        public static Vehicle MapToVehicle(this UpdateVehicle command) => new Vehicle
        {
            LicenseNumber = command.LicenseNumber,
            Brand = command.Brand,
            Type = command.Type,
            OwnerId = command.OwnerId
        };
        public static VehicleRegistered MapToVehicleRegistered(RegisterVehicle command)
        {
            return new VehicleRegistered(
                Guid.NewGuid(),
                command.LicenseNumber,
                command.Brand,
                command.Type,
                command.OwnerId
            );
        }

        public static VehicleUpdated MapToVehicleUpdated(UpdateVehicle command)
        {
            return new VehicleUpdated(
                Guid.NewGuid(),
                command.LicenseNumber,
                command.Brand,
                command.Type,
                command.OwnerId
            );
        }
    }
}