using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.WorkshopManagementAPI.Domain
{
    public class Customer
    {
        public string CustomerId { get; private set; }
        public string Name { get; private set; }
        public string TelephoneNumber { get; private set; }

        public Customer(string customerId, string name, string telephoneNumber)
        {
            CustomerId = customerId;
            Name = name;
            TelephoneNumber = telephoneNumber;
        }
    }
}
