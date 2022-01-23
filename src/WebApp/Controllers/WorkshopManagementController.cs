namespace PitStop.WebApp.Controllers;

    public class WorkshopManagementController : Controller
{
    private IWorkshopManagementAPI _workshopManagementAPI;
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    private ResiliencyHelper _resiliencyHelper;

    public WorkshopManagementController(IWorkshopManagementAPI workshopManagamentAPI, ILogger<WorkshopManagementController> logger)
    {
        _workshopManagementAPI = workshopManagamentAPI;
        _logger = logger;
        _resiliencyHelper = new ResiliencyHelper(_logger);
    }

    [HttpGet]
    public async Task<IActionResult> Index(DateTime? planningDate)
    {
        return await _resiliencyHelper.ExecuteResilient(async () =>
        {
            if (planningDate == null)
            {
                planningDate = DateTime.Now.Date;
            }

            var model = new WorkshopManagementViewModel
            {
                Date = planningDate.Value,
                MaintenanceJobs = new List<MaintenanceJob>()
            };

                // get planning
                string dateStr = planningDate.Value.ToString("yyyy-MM-dd");
            WorkshopPlanning planning = await _workshopManagementAPI.GetWorkshopPlanning(dateStr);
            if (planning?.Jobs?.Count > 0)
            {
                model.MaintenanceJobs.AddRange(planning.Jobs.OrderBy(j => j.StartTime));
            }

            return View(model);
        }, View("Offline", new WorkshopManagementOfflineViewModel()));
    }

    [HttpGet]
    public async Task<IActionResult> Details(DateTime planningDate, string jobId)
    {
        return await _resiliencyHelper.ExecuteResilient(async () =>
        {
            string dateStr = planningDate.ToString("yyyy-MM-dd");
            var model = new WorkshopManagementDetailsViewModel
            {
                Date = planningDate,
                MaintenanceJob = await _workshopManagementAPI.GetMaintenanceJob(dateStr, jobId)
            };
            return View(model);
        }, View("Offline", new WorkshopManagementOfflineViewModel()));
    }

    [HttpGet]
    public async Task<IActionResult> New(DateTime planningDate)
    {
        return await _resiliencyHelper.ExecuteResilient(async () =>
        {
            DateTime startTime = planningDate.Date.AddHours(8);

            var model = new WorkshopManagementNewViewModel
            {
                Id = Guid.NewGuid(),
                Date = planningDate,
                StartTime = startTime,
                EndTime = startTime.AddHours(2),
                Vehicles = await GetAvailableVehiclesList()
            };
            return View(model);
        }, View("Offline", new WorkshopManagementOfflineViewModel()));
    }

    [HttpGet]
    public async Task<IActionResult> Finish(DateTime planningDate, string jobId)
    {
        return await _resiliencyHelper.ExecuteResilient(async () =>
        {
            string dateStr = planningDate.ToString("yyyy-MM-dd");
            MaintenanceJob job = await _workshopManagementAPI.GetMaintenanceJob(dateStr, jobId);
            var model = new WorkshopManagementFinishViewModel
            {
                Id = job.Id,
                Date = planningDate,
                ActualStartTime = job.StartTime,
                ActualEndTime = job.EndTime
            };
            return View(model);
        }, View("Offline", new WorkshopManagementOfflineViewModel()));
    }

    [HttpPost]
    public async Task<IActionResult> RegisterMaintenanceJob([FromForm] WorkshopManagementNewViewModel inputModel)
    {
        if (ModelState.IsValid)
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                string dateStr = inputModel.Date.ToString("yyyy-MM-dd");

                try
                {
                        // register maintenance job
                        DateTime startTime = inputModel.Date.Add(inputModel.StartTime.TimeOfDay);
                    DateTime endTime = inputModel.Date.Add(inputModel.EndTime.TimeOfDay);
                    Vehicle vehicle = await _workshopManagementAPI.GetVehicleByLicenseNumber(inputModel.SelectedVehicleLicenseNumber);
                    Customer customer = await _workshopManagementAPI.GetCustomerById(vehicle.OwnerId);

                    PlanMaintenanceJob planMaintenanceJobCommand = new PlanMaintenanceJob(Guid.NewGuid(), Guid.NewGuid(), startTime, endTime,
                        (customer.CustomerId, customer.Name, customer.TelephoneNumber),
                        (vehicle.LicenseNumber, vehicle.Brand, vehicle.Type), inputModel.Description);
                    await _workshopManagementAPI.PlanMaintenanceJob(dateStr, planMaintenanceJobCommand);
                }
                catch (ApiException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.Conflict)
                    {
                            // add errormessage from API exception to model
                            var content = await ex.GetContentAsAsync<BusinessRuleViolation>();
                        inputModel.Error = content.ErrorMessage;

                            // repopulate list of available vehicles in the model
                            inputModel.Vehicles = await GetAvailableVehiclesList();

                            // back to New view
                            return View("New", inputModel);
                    }
                }

                return RedirectToAction("Index", new { planningDate = dateStr });
            }, View("Offline", new WorkshopManagementOfflineViewModel()));
        }
        else
        {
            inputModel.Vehicles = await GetAvailableVehiclesList();
            return View("New", inputModel);
        }
    }

    [HttpPost]
    public async Task<IActionResult> FinishMaintenanceJob([FromForm] WorkshopManagementFinishViewModel inputModel)
    {
        if (ModelState.IsValid)
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                string dateStr = inputModel.Date.ToString("yyyy-MM-dd");
                DateTime actualStartTime = inputModel.Date.Add(inputModel.ActualStartTime.Value.TimeOfDay);
                DateTime actualEndTime = inputModel.Date.Add(inputModel.ActualEndTime.Value.TimeOfDay);

                FinishMaintenanceJob cmd = new FinishMaintenanceJob(Guid.NewGuid(), inputModel.Id,
                    actualStartTime, actualEndTime, inputModel.Notes);

                await _workshopManagementAPI.FinishMaintenanceJob(dateStr, inputModel.Id.ToString("D"), cmd);

                return RedirectToAction("Details", new { planningDate = dateStr, jobId = inputModel.Id });
            }, View("Offline", new WorkshopManagementOfflineViewModel()));
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
}