﻿using Pitstop.Infrastructure.Messaging;
using System;

namespace Pitstop.Application.VehicleManagement.Commands
{
    public class UpdateVehicle : Command
    {
        public readonly string LicenseNumber;
        public readonly string Brand;
        public readonly string Type;
        public readonly string OwnerId;

        public UpdateVehicle(Guid messageId, string licenseNumber, string brand, string type, string ownerId) :
            base(messageId)
        {
            LicenseNumber = licenseNumber;
            Brand = brand;
            Type = type;
            OwnerId = ownerId;
        }
    }
}