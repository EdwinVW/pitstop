using Pitstop.Infrastructure.Messaging;
using System;

namespace Pitstop.NotificationService.Events
{
    public class CustomerUpdated : Event
    {
        public readonly string CustomerId;
        public readonly string Name;
        public readonly string TelephoneNumber;
        public readonly string EmailAddress;

        public CustomerUpdated(Guid messageId, string customerId, string name, string telephoneNumber, string emailAddress) :
            base(messageId)
        {
            CustomerId = customerId;
            Name = name;
            TelephoneNumber = telephoneNumber;
            EmailAddress = emailAddress;
        }
    }
}
