using Pitstop.InvoiceService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace Pitstop.InvoiceService.Repositories
{
    public interface IInvoiceRepository
    {
        Task RegisterCustomerAsync(Customer customer);
        Task<Customer> GetCustomerAsync(string customerId);
        Task RegisterMaintenanceJobAsync(MaintenanceJob job);
        Task MarkMaintenanceJobAsFinished(string jobId, DateTime startTime, DateTime endTime);
        Task<IEnumerable<MaintenanceJob>> GetMaintenanceJobsToBeInvoicedAsync();
        Task RegisterInvoiceAsync(Invoice invoice);
    }
}
