using Pitstop.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Commands;

namespace WebApp.RESTClients
{
    public interface IWorkshopManagementAPI
    {
        [Get("/workshopplanning/{date}")]
        Task<WorkshopPlanning> GetWorkshopPlanning(string date);

        [Get("/workshopplanning/{date}/jobs/{jobId}")]
        Task<MaintenanceJob> GetMaintenanceJob(string date, string jobId);

        [Post("/workshopplanning/{date}")]
        Task RegisterPlanning(string date);

        [Post("/workshopplanning/{date}/jobs")]
        Task PlanMaintenanceJob(string date, PlanMaintenanceJob cmd);

        [Put("/workshopplanning/{date}/jobs/{jobId}/finish")]
        Task FinishMaintenanceJob(string date, string jobId, FinishMaintenanceJob cmd);

        [Get("/refdata/customers")]
        Task<List<Customer>> GetCustomers();

        [Get("/refdata/customers/{id}")]
        Task<Customer> GetCustomerById(string id);

        [Get("/refdata/vehicles")]
        Task<List<Vehicle>> GetVehicles();

        [Get("/refdata/vehicles/{id}")]
        Task<Vehicle> GetVehicleByLicenseNumber([AliasAs("id")] string licenseNumber);
    }
}
