using System;
using System.Text;
using Pitstop.WorkshopManagementAPI.Domain;
using Pitstop.WorkshopManagementAPI.Domain.Entities;
using WorkshopManagementAPI.Domain.ValueObjects;

namespace WorkshopManagement.UnitTests.TestdataBuilders
{
    public class VehicleBuilder
    {
        private Random _rnd;

        public LicenseNumber LicenseNumber { get; private set; }
        public string Brand { get; private set; }
        public string Type { get; private set; }
        public string OwnerId { get; private set; }

        public VehicleBuilder()
        {
            _rnd = new Random();
            SetDefaults();
        }

        public VehicleBuilder WithLicenseNumber(string licenseNumber)
        {
            LicenseNumber = LicenseNumber.Create(licenseNumber);
            return this;
        }

        public VehicleBuilder WithRandomLicenseNumber()
        {
            LicenseNumber = LicenseNumber.Create(GenerateRandomLicenseNumber());
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

        public VehicleBuilder WithOwnerId(string ownerId)
        {
            OwnerId = ownerId;
            return this;
        }

        public Vehicle Build()
        {
            if (string.IsNullOrEmpty(OwnerId))
            {
                throw new InvalidOperationException("You must specify an owner id using the 'WithOwnerId' method.");
            }
            return new Vehicle(LicenseNumber, Brand, Type, OwnerId);
        }

        private void SetDefaults()
        {
            LicenseNumber = LicenseNumber.Create(GenerateRandomLicenseNumber());
            Brand = "Volkswagen";
            Type = "Tiguan";
        }

        private string _validLicenseNumberChars = "DFGHJKLNPRSTXYZ";

        /// <summary>
        /// Generate random licensenumber.
        /// </summary>
        private string GenerateRandomLicenseNumber()
        {
            int type = _rnd.Next(1, 9);
            string kenteken = null;
            switch (type)
            {
                case 1: // 99-AA-99
                    kenteken = string.Format("{0:00}-{1}-{2:00}", _rnd.Next(1, 99), GenerateRandomCharacters(2), _rnd.Next(1, 99));
                    break;
                case 2: // AA-99-AA
                    kenteken = string.Format("{0}-{1:00}-{2}", GenerateRandomCharacters(2), _rnd.Next(1, 99), GenerateRandomCharacters(2));
                    break;
                case 3: // AA-AA-99
                    kenteken = string.Format("{0}-{1}-{2:00}", GenerateRandomCharacters(2), GenerateRandomCharacters(2), _rnd.Next(1, 99));
                    break;
                case 4: // 99-AA-AA
                    kenteken = string.Format("{0:00}-{1}-{2}", _rnd.Next(1, 99), GenerateRandomCharacters(2), GenerateRandomCharacters(2));
                    break;
                case 5: // 99-AAA-9
                    kenteken = string.Format("{0:00}-{1}-{2}", _rnd.Next(1, 99), GenerateRandomCharacters(3), _rnd.Next(1, 10));
                    break;
                case 6: // 9-AAA-99
                    kenteken = string.Format("{0}-{1}-{2:00}", _rnd.Next(1, 9), GenerateRandomCharacters(3), _rnd.Next(1, 10));
                    break;
                case 7: // AA-999-A
                    kenteken = string.Format("{0}-{1:000}-{2}", GenerateRandomCharacters(2), _rnd.Next(1, 999), GenerateRandomCharacters(1));
                    break;
                case 8: // A-999-AA
                    kenteken = string.Format("{0}-{1:000}-{2}", GenerateRandomCharacters(1), _rnd.Next(1, 999), GenerateRandomCharacters(2));
                    break;
            }

            return kenteken;
        }

        private string GenerateRandomCharacters(int aantal)
        {
            char[] chars = new char[aantal];
            for (int i = 0; i < aantal; i++)
            {
                chars[i] = _validLicenseNumberChars[_rnd.Next(_validLicenseNumberChars.Length - 1)];
            }
            return new string(chars);
        }
    }
}