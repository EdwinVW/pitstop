using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Models;
using Microsoft.AspNetCore.Hosting;
using Refit;
using WebApp.Commands;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;

namespace WebApp.RESTClients
{
    public class VehicleManagementAPI : IVehicleManagementAPI
    {
        private IVehicleManagementAPI _restClient;

        public  VehicleManagementAPI(IConfiguration config, HttpClient httpClient)
        {
            string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("VehicleManagementAPI");
            httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}/api");
            _restClient = RestService.For<IVehicleManagementAPI>(httpClient);
        }

        public async Task<List<Vehicle>> GetVehicles()
        {
            return await _restClient.GetVehicles();
        }
        public async Task<Vehicle> GetVehicleByLicenseNumber([AliasAs("id")] string licenseNumber)
        {
            try
            {
                return await _restClient.GetVehicleByLicenseNumber(licenseNumber);
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

        public async Task RegisterVehicle(RegisterVehicle command)
        {
            await _restClient.RegisterVehicle(command);
        }
    }
}
