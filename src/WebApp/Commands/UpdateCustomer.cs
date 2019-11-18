using Pitstop.Infrastructure.Messaging;
using System;

namespace WebApp.Commands
{
    public class UpdateCustomer : Command
    {
        public readonly string CustomerId;
        public readonly string Name;
        public readonly string Address;
        public readonly string PostalCode;
        public readonly string City;
        public readonly string TelephoneNumber;
        public readonly string EmailAddress;

        public UpdateCustomer(Guid messageId, string customerId, string name, string address, string postalCode, string city,
            string telephoneNumber, string emailAddress) : base(messageId)
        {
            CustomerId = customerId;
            Name = name;
            Address = address;
            PostalCode = postalCode;
            City = city;
            TelephoneNumber = telephoneNumber;
            EmailAddress = emailAddress;
        }
    }
}
