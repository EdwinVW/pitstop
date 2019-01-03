using System;
using Pitstop.WorkshopManagementAPI.Domain;

namespace WorkshopManagement.UnitTests.TestdataBuilders
{
    public class VehicleBuilder
    {
        public string LicenseNumber { get; private set; }
        public string Brand { get; private set; }
        public string Type { get; private set; }
        public string CustomerId { get; private set; }

        public VehicleBuilder()
        {
            SetDefaults();
        }

        public VehicleBuilder WithLicenseNumber(string licenseNumber)
        {
            LicenseNumber = licenseNumber;
            return this;
        }

        public VehicleBuilder WithBrand(string brand)
        {
            Brand = brand;
            return this;
        }

        public VehicleBuilder WithType(string type)
        {
            Type = type;
            return this;
        }

        public VehicleBuilder WithCustomerId(string customerId)
        {
            CustomerId = customerId;
            return this;
        }

        public Vehicle Build()
        {
            if (string.IsNullOrEmpty(CustomerId))
            {
                throw new InvalidOperationException("You must specify a customer id using the 'WithCustomerId' method.");
            }
            return new Vehicle(LicenseNumber, Brand, Type, CustomerId);
        }

        private void SetDefaults()
        {
            LicenseNumber = Guid.NewGuid().ToString();
            Brand = "Volkswagen";
            Type = "Tiguan";
        }
    }
}