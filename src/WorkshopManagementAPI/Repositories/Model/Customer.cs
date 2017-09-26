using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.WorkshopManagementAPI.Repositories.Model
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string TelephoneNumber { get; set; }
    }
}
