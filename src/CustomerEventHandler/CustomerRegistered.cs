
using System;
using Pitstop.Infrastructure.Messaging;

namespace CustomerEventHandler
{
    public class CustomerRegistered : Event
    {
        public Guid customerId { get; set; }  
        public string Name { get; set; }   
        public CustomerRegistered()
        {
            
        }
    }
}