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
            
            var customerVehicles = new List<RepairManagementViewModel.CustomerVehicleViewModel>();
            _logger.LogInformation("Getting customer vehicles");
            foreach (var vehicle in vehicles)
            {
                var customer = await _customerManagementApi.GetCustomerById(vehicle.OwnerId);
                
                var repairOrder = repairOrders.FirstOrDefault(ro => ro.CustomerId == customer.CustomerId && ro.LicenseNumber == vehicle.LicenseNumber);
                
                var status = repairOrder != null ? repairOrder.Status.ToString() : RepairOrdersStatus.NotCreatedYet.ToString();
                
                customerVehicles.Add(new RepairManagementViewModel.CustomerVehicleViewModel
                {
                    CustomerName = customer.Name,
                    LicenseNumber = vehicle.LicenseNumber,
                    RepairOrderStatus = status
                });
            }
            _logger.LogInformation("createing customer vehicles");
            var model = new RepairManagementViewModel
            {
                CustomerVehicles = customerVehicles
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


    [HttpGet]
    public IActionResult New()
    {
        return View();
    }
}