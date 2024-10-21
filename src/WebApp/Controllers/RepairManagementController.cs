using Pitstop.WebApp.RESTClients;

namespace PitStop.WebApp.Controllers;

public class RepairManagementController : Controller
{
    private readonly IVehicleManagementAPI _vehicleManagementApi;
    private readonly ICustomerManagementAPI _customerManagementApi;
    private readonly IRepairManagementAPI _repairManagementApi;
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    private readonly ResiliencyHelper _resiliencyHelper;

    public RepairManagementController(IVehicleManagementAPI vehicleManagementApi,
        ICustomerManagementAPI customerManagementApi, IRepairManagementAPI repairManagementApi,
        ILogger<RepairManagementController> logger)
    {
        _vehicleManagementApi = vehicleManagementApi;
        _customerManagementApi = customerManagementApi;
        _repairManagementApi = repairManagementApi;
        _logger = logger;
        _resiliencyHelper = new ResiliencyHelper(_logger);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Getting vehicle list");
        return await _resiliencyHelper.ExecuteResilient(async () =>
        {
            var repairOrders = await _repairManagementApi.GetAllRepairOrders();
            var vehicles = await _vehicleManagementApi.GetVehicles();

            var customerVehicles = new List<RepairManagementViewModel.RepairManagementCustomerVehicleViewModel>();
            _logger.LogInformation("Getting customer vehicles");

            foreach (var vehicle in vehicles)
            {
                RepairOrders repairOrder = null;
                var status = "";
                var customer = await _customerManagementApi.GetCustomerById(vehicle.OwnerId);

                // Check if repairOrders exist before accessing them
                if (repairOrders != null && repairOrders.Any())
                {
                    repairOrder = repairOrders.FirstOrDefault(ro =>
                        ro.CustomerId == customer.CustomerId && ro.LicenseNumber == vehicle.LicenseNumber);
                    if (repairOrder != null)
                    {
                        status = repairOrder.Status.ToString();
                    }
                }
                else
                {
                    _logger.LogInformation(
                        $"No repair orders found for vehicle {vehicle.LicenseNumber}. Setting status to 'NotCreatedYet'");
                    status = "NotCreatedYet";
                }

                _logger.LogInformation(
                    $"Customer: {customer.Name}, Vehicle: {vehicle.LicenseNumber}, Status: {status}");

                customerVehicles.Add(new RepairManagementViewModel.RepairManagementCustomerVehicleViewModel
                {
                    CustomerId = vehicle.OwnerId,
                    CustomerName = customer.Name,
                    LicenseNumber = vehicle.LicenseNumber,
                    RepairOrderStatus = status
                });
            }

            _logger.LogInformation("Creating customer vehicles list.");
            var model = new RepairManagementViewModel
            {
                RepairOrders = customerVehicles
            };

            return View(model);
        }, View("Offline", new RepairManagementOfflineViewModel()));
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Details()
    {
        return View();
    }


    [HttpGet("New")]
    public async Task<IActionResult> New(string customerId, string licenseNumber)
    {
        _logger.LogInformation(
            $"Loading New Repair Order page for customerId: {customerId}, licenseNumber: {licenseNumber}");

        // Get customer details
        var customer = await _customerManagementApi.GetCustomerById(customerId);

        // Get available vehicle parts from the API
        var availableVehicleParts = await _repairManagementApi.GetVehicleParts();

        if (availableVehicleParts == null || !availableVehicleParts.Any())
        {
            availableVehicleParts = new List<VehicleParts>();
        }


        // Create the view model and populate with data
        var model = new RepairManagementNewViewModel
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.Name,
            LicenseNumber = licenseNumber,
            AvailableVehicleParts = availableVehicleParts,
            SelectedVehicleParts = new List<string>(),
        };
        return View(model);
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> CreateRepairOrder(RepairManagementNewViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Retrieve the vehicle parts again if model state is invalid
            var availableVehicleParts = await _repairManagementApi.GetVehicleParts();
            model.AvailableVehicleParts = availableVehicleParts;

            return View("New", model);
        }

        var repairOrderId = Guid.NewGuid().ToString();

        // Prepare the CreateRepairOrder command
        var command = new CreateRepairOrder(
            messageId: Guid.NewGuid(),
            repairOrderId: repairOrderId,
            customerId: model.CustomerId,
            licenseNumber: model.LicenseNumber,
            vehiclePartId: model.SelectedVehicleParts, // IDs of the selected parts
            totalCost: model.TotalCost,
            laborCost: model.LaborCost, // Labor cost is already decimal
            isApproved: false, // New repair orders are not approved by default
            createdAt: DateTime.Now,
            status: RepairOrdersStatus.Sent
        );

        // Send the command to the RepairManagementAPI
        await _repairManagementApi.CreateRepairOrder(command);

        // Redirect to the Index page after successful creation
        return RedirectToAction("Index");
    }
}