using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitstop.Models;
using Pitstop.ViewModels;
using AutoMapper;
using Polly;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Commands;
using WebApp.RESTClients;

namespace PitStop.Controllers
{
    public class VehicleManagementController : Controller
    {
        private IVehicleManagementAPI _vehicleManagementAPI;
        private ICustomerManagementAPI _customerManagementAPI;
        private readonly ILogger _logger;

        public VehicleManagementController(IVehicleManagementAPI vehicleManagementAPI, 
            ICustomerManagementAPI customerManagementAPI, ILogger<VehicleManagementController> logger)
        {
            _vehicleManagementAPI = vehicleManagementAPI;
            _customerManagementAPI = customerManagementAPI;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await ExecuteWithFallback(async () =>
            {
                var model = new VehicleManagementViewModel
                {
                    Vehicles = await _vehicleManagementAPI.GetVehicles()
                };
                return View(model);
            });
        }

        [HttpGet]
        public async Task<IActionResult> Details(string licenseNumber)
        {
            return await ExecuteWithFallback(async () =>
            {
                Vehicle vehicle = await _vehicleManagementAPI.GetVehicleByLicenseNumber(licenseNumber);
                Customer customer = await _customerManagementAPI.GetCustomerById(vehicle.OwnerId);

                var model = new VehicleManagementDetailsViewModel
                {
                    Vehicle = vehicle,
                    Owner = customer.Name
                };
                return View(model);
            });
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            return await ExecuteWithFallback(async () =>
            {
                // get customerlist
                var customers = await _customerManagementAPI.GetCustomers();

                var model = new VehicleManagementNewViewModel
                {
                    Vehicle = new Vehicle(),
                    Customers = customers.Select(c => new SelectListItem { Value = c.CustomerId, Text = c.Name })
                };
                return View(model);
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] VehicleManagementNewViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                return await ExecuteWithFallback(async () =>
                {
                    RegisterVehicle cmd = Mapper.Map<RegisterVehicle>(inputModel);
                    await _vehicleManagementAPI.RegisterVehicle(cmd);
                    return RedirectToAction("Index");
                });
            }
            else
            {
                return View("New", inputModel);
            }
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        private async Task<IActionResult> ExecuteWithFallback(Func<Task<IActionResult>> action)
        {
            return await Policy<IActionResult>
                .Handle<Exception>()
                .FallbackAsync(
                    View("Offline", new VehicleManagementOfflineViewModel()), 
                    (e, c) => Task.Run(() => _logger.LogError(e.Exception.ToString())))
                .ExecuteAsync(action);
        }
    }
}
