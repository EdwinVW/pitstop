using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Models;
using Microsoft.AspNetCore.Hosting;
using Refit;
using WebApp.Commands;
using System;
using System.Net;

namespace WebApp.RESTClients
{
    public class WorkshopManagementAPI : IWorkshopManagementAPI
    {
        private IWorkshopManagementAPI _client;

        public WorkshopManagementAPI(IHostingEnvironment env)
        {
            string apiHost = env.IsDevelopment() ? "localhost" : "workshopmanagementapi";
            int apiPort = 5200;
            string baseUri = $"http://{apiHost}:{apiPort}/api";
            _client = RestService.For<IWorkshopManagementAPI>(baseUri);
        }

        public async Task<WorkshopPlanning> GetWorkshopPlanning(string date)
        {
            try
            {
                return await _client.GetWorkshopPlanning(date);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<MaintenanceJob> GetMaintenanceJob(string date, string jobId)
        {
            try
            {
                return await _client.GetMaintenanceJob(date, jobId);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task RegisterPlanning(string date)
        {
            await _client.RegisterPlanning(date);
        }

        public async Task PlanMaintenanceJob(string date, PlanMaintenanceJob cmd)
        {
            await _client.PlanMaintenanceJob(date, cmd);
        }

        public async Task FinishMaintenanceJob(string date, string jobId, FinishMaintenanceJob cmd)
        {
            await _client.FinishMaintenanceJob(date, jobId, cmd);
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await _client.GetCustomers();
        }

        public async Task<Customer> GetCustomerById(string id)
        {
            try
            {
                return await _client.GetCustomerById(id);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<List<Vehicle>> GetVehicles()
        {
            return await _client.GetVehicles();
        }

        public async Task<Vehicle> GetVehicleByLicenseNumber([AliasAs("id")] string licenseNumber)
        {
            try
            {
                return await _client.GetVehicleByLicenseNumber(licenseNumber);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
