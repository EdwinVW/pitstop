using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitstop.Models;
using Pitstop.ViewModels;
using Polly;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Commands;
using WebApp.RESTClients;
using Refit;
using System.Net;
using WebApp.Models;

namespace PitStop.Controllers
{
    public class WorkshopManagementController : Controller
    {
        private IWorkshopManagementAPI _workshopManagementAPI;
        private readonly ILogger _logger;

        public WorkshopManagementController(IWorkshopManagementAPI workshopManagamentAPI, ILogger<WorkshopManagementController> logger)
        {
            _workshopManagementAPI = workshopManagamentAPI;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? date)
        {
            return await ExecuteWithFallback(async () =>
            {
                if (date == null)
                {
                    date = DateTime.Now.Date;
                }

                var model = new WorkshopManagementViewModel
                {
                    Date = date.Value,
                    MaintenanceJobs = new List<MaintenanceJob>()
                };

                // get planning
                string dateStr = date.Value.ToString("yyyy-MM-dd");
                WorkshopPlanning planning = await _workshopManagementAPI.GetWorkshopPlanning(dateStr);
                if (planning?.Jobs?.Count > 0)
                {
                    model.MaintenanceJobs.AddRange(planning.Jobs.OrderBy(j => j.StartTime));
                }

                return View(model);
            });
        }

        [HttpGet]
        public async Task<IActionResult> Details(DateTime date, string jobId)
        {
            return await ExecuteWithFallback(async () =>
            {
                string dateStr = date.ToString("yyyy-MM-dd");
                var model = new WorkshopManagementDetailsViewModel
                {
                    Date = date,
                    MaintenanceJob = await _workshopManagementAPI.GetMaintenanceJob(dateStr, jobId)
                };
                return View(model);
            });
        }

        [HttpGet]
        public async Task<IActionResult> New(DateTime date)
        {
            return await ExecuteWithFallback(async () =>
            {
                DateTime startTime = date.Date.AddHours(8);

                var model = new WorkshopManagementNewViewModel
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(2),
                    Vehicles = await GetAvailableVehiclesList()
                };
                return View(model);
            });
        }

        [HttpGet]
        public async Task<IActionResult> Finish(DateTime date, string jobId)
        {
            return await ExecuteWithFallback(async () =>
            {
                string dateStr = date.ToString("yyyy-MM-dd");
                MaintenanceJob job = await _workshopManagementAPI.GetMaintenanceJob(dateStr, jobId);
                var model = new WorkshopManagementFinishViewModel
                {
                    Id = job.Id,
                    Date = date,
                    ActualStartTime = job.StartTime,
                    ActualEndTime = job.EndTime
                };
                return View(model);
            });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterMaintenanceJob([FromForm] WorkshopManagementNewViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                return await ExecuteWithFallback(async () =>
                {
                    try
                    {
                        string dateStr = inputModel.Date.ToString("yyyy-MM-dd");

                    // get or create planning for date
                    var planning = await _workshopManagementAPI.GetWorkshopPlanning(dateStr);
                        if (planning == null)
                        {
                        // create planning for date
                        await _workshopManagementAPI.RegisterPlanning(dateStr);
                        }

                    // register maintenance job
                    DateTime startTime = inputModel.Date.Add(inputModel.StartTime.TimeOfDay);
                        DateTime endTime = inputModel.Date.Add(inputModel.EndTime.TimeOfDay);
                        Vehicle vehicle = await _workshopManagementAPI.GetVehicleByLicenseNumber(inputModel.SelectedVehicleLicenseNumber);
                        Customer customer = await _workshopManagementAPI.GetCustomerById(vehicle.OwnerId);

                        PlanMaintenanceJob cmd = new PlanMaintenanceJob(Guid.NewGuid(), Guid.NewGuid(), startTime, endTime,
                            (customer.CustomerId, customer.Name, customer.TelephoneNumber),
                            (vehicle.LicenseNumber, vehicle.Brand, vehicle.Type), inputModel.Description);
                        await _workshopManagementAPI.PlanMaintenanceJob(dateStr, cmd);
                    }
                    catch (ApiException ex)
                    {
                        if (ex.StatusCode == HttpStatusCode.Conflict)
                        {
                        // add errormessage from API exception to model
                        var content = ex.GetContentAs<BusinessRuleViolation>();
                            inputModel.Error = content.ErrorMessage;

                        // repopulate list of available vehicles in the model
                        inputModel.Vehicles = await GetAvailableVehiclesList();

                        // back to New view
                        return View("New", inputModel);
                        }
                    }

                    return RedirectToAction("Index", new { date = inputModel.Date });
                });
            }
            else
            {
                inputModel.Vehicles = await GetAvailableVehiclesList();
                return View("New", inputModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> FinishMaintenanceJob(DateTime date, [FromForm] WorkshopManagementFinishViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                return await ExecuteWithFallback(async () =>
                {
                    string dateStr = inputModel.Date.ToString("yyyy-MM-dd");
                    DateTime actualStartTime = inputModel.Date.Add(inputModel.ActualStartTime.Value.TimeOfDay);
                    DateTime actualEndTime = inputModel.Date.Add(inputModel.ActualEndTime.Value.TimeOfDay);

                    FinishMaintenanceJob cmd = new FinishMaintenanceJob(Guid.NewGuid(), inputModel.Id,
                        actualStartTime, actualEndTime, inputModel.Notes);

                    await _workshopManagementAPI.FinishMaintenanceJob(dateStr, inputModel.Id.ToString("D"), cmd);

                    return RedirectToAction("Details", new { date = date, jobId = inputModel.Id });
                });
            }
            else
            {
                return View("Finish", inputModel);
            }
        }

        public IActionResult Error()
        {
            return View();
        }

        private async Task<IEnumerable<SelectListItem>> GetAvailableVehiclesList()
        {
            var vehicles = await _workshopManagementAPI.GetVehicles();
            return vehicles.Select(v =>
                new SelectListItem
                {
                    Value = v.LicenseNumber,
                    Text = $"{v.Brand} {v.Type} [{v.LicenseNumber}]"
                });
        }

        private async Task<IActionResult> ExecuteWithFallback(Func<Task<IActionResult>> action)
        {
            IActionResult fallbackResult = View("Offline", new WorkshopManagementOfflineViewModel());
            return await Policy<IActionResult>
                .Handle<Exception>()
                .FallbackAsync(
                    fallbackResult,
                    (e, c) => Task.Run(() => _logger.LogError(e.Exception.ToString())))
                .ExecuteAsync(action);
        }
    }
}
