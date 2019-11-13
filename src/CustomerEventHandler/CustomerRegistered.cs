
using System;
using Pitstop.Infrastructure.Messaging;

namespace CustomerEventHandler
{
    public class CustomerRegistered : Event
    {
        public string CustomerId { get; set; }  
        public string Name { get; set; }   

        public CustomerRegistered(Guid messageId, string customerId, string name) : base(messageId)
        {
            CustomerId = customerId;
            Name = name;
        }
    }
}