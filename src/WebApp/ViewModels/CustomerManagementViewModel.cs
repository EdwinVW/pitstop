using Pitstop.Models;
using System.Collections.Generic;

namespace Pitstop.ViewModels
{
    public class CustomerManagementViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
    }
}
