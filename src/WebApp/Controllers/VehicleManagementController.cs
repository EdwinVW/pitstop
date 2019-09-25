using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitstop.Models;
using Pitstop.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Commands;
using WebApp.RESTClients;
using Pitstop.WebApp.Mappers;

namespace PitStop.Controllers
{
    public class VehicleManagementController : Controller
    {
        private IVehicleManagementAPI _vehicleManagementAPI;
        private ICustomerManagementAPI _customerManagementAPI;
        private readonly ILogger _logger;
        private ResiliencyHelper _resiliencyHelper;

        public VehicleManagementController(IVehicleManagementAPI vehicleManagementAPI, 
            ICustomerManagementAPI customerManagementAPI, ILogger<VehicleManagementController> logger)
        {
            _vehicleManagementAPI = vehicleManagementAPI;
            _customerManagementAPI = customerManagementAPI;
            _logger = logger;
            _resiliencyHelper = new ResiliencyHelper(_logger);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                var model = new VehicleManagementViewModel
                {
                    Vehicles = await _vehicleManagementAPI.GetVehicles()
                };
                return View(model);
            }, View("Offline", new VehicleManagementOfflineViewModel()));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string licenseNumber)
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                Vehicle vehicle = await _vehicleManagementAPI.GetVehicleByLicenseNumber(licenseNumber);
                Customer customer = await _customerManagementAPI.GetCustomerById(vehicle.OwnerId);

                var model = new VehicleManagementDetailsViewModel
                {
                    Vehicle = vehicle,
                    Owner = customer.Name
                };
                return View(model);
            }, View("Offline", new VehicleManagementOfflineViewModel()));
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                // get customerlist
                var customers = await _customerManagementAPI.GetCustomers();

                var model = new VehicleManagementNewViewModel
                {
                    Vehicle = new Vehicle(),
                    Customers = customers.Select(c => new SelectListItem { Value = c.CustomerId, Text = c.Name })
                };
                return View(model);
            }, View("Offline", new VehicleManagementOfflineViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] VehicleManagementNewViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                return await _resiliencyHelper.ExecuteResilient(async () =>
                {
                    RegisterVehicle cmd = inputModel.MapToRegisterVehicle();
                    await _vehicleManagementAPI.RegisterVehicle(cmd);
                    return RedirectToAction("Index");
                }, View("Offline", new VehicleManagementOfflineViewModel()));
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
    }
}
