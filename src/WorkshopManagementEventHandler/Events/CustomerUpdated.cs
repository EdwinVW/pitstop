using Pitstop.Infrastructure.Messaging;
using System;

namespace Pitstop.WorkshopManagementEventHandler.Events
{
    public class CustomerUpdated : Event
    {
        public readonly string CustomerId;
        public readonly string Name;
        public readonly string TelephoneNumber;

        public CustomerUpdated(Guid messageId, string customerId, string name, string telephoneNumber) :
            base(messageId)
        {
            CustomerId = customerId;
            Name = name;
            TelephoneNumber = telephoneNumber;
        }
    }
}
