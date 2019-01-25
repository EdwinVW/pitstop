using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Models;
using Microsoft.AspNetCore.Hosting;
using Refit;
using WebApp.Commands;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace WebApp.RESTClients
{
    public class CustomerManagementAPI : ICustomerManagementAPI
    {
        private ICustomerManagementAPI _client;

        public  CustomerManagementAPI(IConfiguration config)
        {
            string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("CustomerManagementAPI");
            string baseUri = $"http://{apiHostAndPort}/api";
            _client = RestService.For<ICustomerManagementAPI>(baseUri);
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await _client.GetCustomers();
        }

        public async Task<Customer> GetCustomerById([AliasAs("id")] string customerId)
        {
            try
            {
                return await _client.GetCustomerById(customerId);
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

        public async Task RegisterCustomer(RegisterCustomer command)
        {
            await _client.RegisterCustomer(command);
        }
    }
}
