using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.WorkshopManagementEventHandler.Events
{
    public class CustomerRegistered : Event
    {
        public readonly string CustomerId;
        public readonly string Name;
        public readonly string TelephoneNumber;

        public CustomerRegistered(Guid messageId, string customerId, string name, string telephoneNumber) : 
            base(messageId)
        {
            CustomerId = customerId;
            Name = name;
            TelephoneNumber = telephoneNumber;
        }
    }
}
