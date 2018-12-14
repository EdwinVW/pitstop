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
            string apiHost = env.IsDevelopment() ? "localhost" : "apigateway";
            int apiPort = 10000;
            string baseUri = $"http://{apiHost}:{apiPort}/api";
            _client = RestService.For<IWorkshopManagementAPI>(baseUri);
        }

        public async Task<WorkshopPlanning> GetWorkshopPlanning(string planningDate)
        {
            try
            {
                return await _client.GetWorkshopPlanning(planningDate);
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

        public async Task<MaintenanceJob> GetMaintenanceJob(string planningDate, string jobId)
        {
            try
            {
                return await _client.GetMaintenanceJob(planningDate, jobId);
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

        public async Task RegisterPlanning(string planningDate, RegisterPlanning cmd)
        {
            await _client.RegisterPlanning(planningDate, cmd);
        }

        public async Task PlanMaintenanceJob(string planningDate, PlanMaintenanceJob cmd)
        {
            await _client.PlanMaintenanceJob(planningDate, cmd);
        }

        public async Task FinishMaintenanceJob(string planningDate, string jobId, FinishMaintenanceJob cmd)
        {
            await _client.FinishMaintenanceJob(planningDate, jobId, cmd);
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
