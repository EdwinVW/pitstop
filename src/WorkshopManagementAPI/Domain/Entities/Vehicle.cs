using Pitstop.WorkshopManagementAPI.Domain.Core;

namespace Pitstop.WorkshopManagementAPI.Domain.Entities
{
    public class Vehicle : Entity<string>
    {
        public string Brand { get; private set; }
        public string Type { get; private set; }
        public string OwnerId { get; private set; }

        public Vehicle(string licenseNumber, string brand, string type, string ownerId) : base(licenseNumber)
        {
            Brand = brand;
            Type = type;
            OwnerId = ownerId;
        }
    }
}
