namespace Pitstop.RepairManagemenAPI.Commands
{
    public class SendCustomerNotification : Command
    {
        public readonly Guid notificationId;
        public readonly DateTime SentAt;
        public readonly (string Id, string Name, string TelephoneNumber, string EmailAddress) CustomerInfo;
        public readonly (string LicenseNumber, string Brand, string Type) VehicleInfo;
        public readonly Part[] Parts;

        public SendCustomerNotification(
            Guid messageId,
            notificationId
            DateTime sentAt,
            (string Id, string Name, string TelephoneNumber, string EmailAddress) customerInfo,
            (string LicenseNumber, string Brand, string Type) vehicleInfo,
            Part[] parts
        ) : base(messageId)
        {
            notificationId = notificationId;
            SentAt = sentAt;
            CustomerInfo = customerInfo;
            VehicleInfo = vehicleInfo;
            Parts = parts;
        }

        public class Part
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }

            public Part(string id, string name, decimal price)
            {
                Id = id;
                Name = name;
                Price = price;
            }
        }
    }
}