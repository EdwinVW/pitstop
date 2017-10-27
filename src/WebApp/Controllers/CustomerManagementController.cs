using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pitstop.Models;
using Pitstop.ViewModels;
using Polly;
using System;
using System.Threading.Tasks;
using WebApp.Commands;
using WebApp.RESTClients;

namespace WebApp.Controllers
{
    public class CustomerManagementController : Controller
    {
        private readonly ICustomerManagementAPI _customerManagementAPI;
        private readonly ILogger _logger;

        public CustomerManagementController(ICustomerManagementAPI customerManagementAPI, ILogger<CustomerManagementController> logger)
        {
            _customerManagementAPI = customerManagementAPI;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await ExecuteWithFallback(async () =>
            {
                var model = new CustomerManagementViewModel
                {
                    Customers = await _customerManagementAPI.GetCustomers()
                };
                return View(model);
            });
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            return await ExecuteWithFallback(async () =>
            {
                var model = new CustomerManagementDetailsViewModel
                {
                    Customer = await _customerManagementAPI.GetCustomerById(id)
                };
                return View(model);
            });
        }

        [HttpGet]
        public IActionResult New()
        {
            var model = new CustomerManagementNewViewModel
            {
                Customer = new Customer()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] CustomerManagementNewViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                return await ExecuteWithFallback(async () =>
                {
                    RegisterCustomer cmd = Mapper.Map<RegisterCustomer>(inputModel.Customer);
                    await _customerManagementAPI.RegisterCustomer(cmd);
                    return RedirectToAction("Index");
                });
            }
            else
            {
                return View("New", inputModel);
            }
        }

        private async Task<IActionResult> ExecuteWithFallback(Func<Task<IActionResult>> action)
        {
            return await Policy<IActionResult>
                .Handle<Exception>()
                .FallbackAsync(
                    View("Offline", new CustomerManagementOfflineViewModel()),
                    (e, c) => Task.Run(() => _logger.LogError(e.Exception.ToString())))
                .ExecuteAsync(action);
        }
    }
}