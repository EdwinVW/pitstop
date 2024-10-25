using Newtonsoft.Json;

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
        return await _resiliencyHelper.ExecuteResilient(async () =>
        {
            var repairOrdersData = await _repairManagementApi.GetAllRepairOrders();
            var vehicles = await _vehicleManagementApi.GetVehicles();

            var repairOrders = new List<RepairOrder>();

            foreach (var v in vehicles)
            {
                var r = repairOrdersData?.FirstOrDefault(ro => ro.VehicleInfo.LicenseNumber == v.LicenseNumber);

                if (r != null)
                {
                    var ro = new RepairOrder
                    {
                        Id = r.Id,
                        CustomerInfo = new CustomerInfo
                        {
                            CustomerName = r.CustomerInfo.CustomerName
                        },
                        VehicleInfo = new VehicleInfo
                        {
                            LicenseNumber = r.VehicleInfo.LicenseNumber,
                        },
                        Status = r.Status
                    };

                    repairOrders.Add(ro);
                }
                else
                {
                    var customer = await _customerManagementApi.GetCustomerById(v.OwnerId);
                    if (customer != null)
                    {
                        var ro = new RepairOrder
                        {
                            CustomerInfo = new CustomerInfo
                            {
                                CustomerName = customer.Name,
                            },
                            VehicleInfo = new VehicleInfo
                            {
                                LicenseNumber = v.LicenseNumber,
                            },
                            Status = RepairOrdersStatus.NotCreatedYet.ToString()
                        };
                        repairOrders.Add(ro);
                    }
                }
            }
            repairOrders = repairOrders.OrderByDescending(ro => ro.Status == RepairOrdersStatus.NotCreatedYet.ToString()).ToList();

            var model = new RepairManagementViewModel
            {
                RepairOrders = repairOrders
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
    public async Task<IActionResult> Details(string id)
    {
        return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                // Fetch the repair order from the API
                var repairOrder = await _repairManagementApi.GetRepairOrderById(id);

                var model = new RepairManagementDetailsViewModel
                {
                    RepairOrder = repairOrder,
                };


                return View(model);
            },
            View("Offline", new RepairManagementOfflineViewModel()));
    }

    [HttpGet]
    public async Task<IActionResult> DetailsCustomer(string id)
    {
        return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                // Fetch the repair order from the API
                var repairOrder = await _repairManagementApi.GetRepairOrderById(id);

                var model = new RepairManagementDetailsCustomerViewModel
                {
                    RepairOrder = repairOrder,
                };


                return View(model);
            },
            View("Offline", new RepairManagementOfflineViewModel()));
    }

    [HttpPost]
    public async Task<IActionResult> ApproveRepairOrder(string repairOrderId)
    {
        try
        {
            await _repairManagementApi.ApproveRepairOrder(repairOrderId);
            TempData["Message"] = "The repair order has been approved successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "An error occurred while approving the repair order.";
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> RejectRepairOrder(RepairManagementRejectRepairOrderViewModel model)
    {
        if (ModelState.IsValid)
        {
            var command = new RejectRepairOrder(new Guid(), model.RejectReason);
            await _repairManagementApi.RejectRepairOrder(model.RepairOrderId, command);
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("DetailsCustomer", new { id = model.RepairOrderId });
    }

    [HttpGet]
    public IActionResult RejectRepairOrder(string repairOrderId)
    {
        var model = new RepairManagementRejectRepairOrderViewModel
        {
            RepairOrderId = repairOrderId,
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> New(string licenseNumber)
    {
        var repairOrder = new RepairOrder();
        repairOrder.Status = RepairOrdersStatus.NotCreatedYet.ToString();
        repairOrder.ToRepairVehiclePartIds = new List<Guid>();

        _logger.LogInformation($"Loading New Repair Order page for licenseNumber: {licenseNumber}");

        var vehicle = await _vehicleManagementApi.GetVehicleByLicenseNumber(licenseNumber);
        var customer = await _customerManagementApi.GetCustomerById(vehicle.OwnerId);

        repairOrder.CustomerInfo = new CustomerInfo
        {
            CustomerName = customer.Name,
            CustomerEmail = customer.EmailAddress,
            CustomerPhone = customer.TelephoneNumber
        };

        repairOrder.VehicleInfo = new VehicleInfo
        {
            LicenseNumber = vehicle.LicenseNumber,
            Brand = vehicle.Brand,
            Type = vehicle.Type
        };

        repairOrder.TotalCost = 0;
        repairOrder.LaborCost = 0;
        repairOrder.IsApproved = false;
        repairOrder.Status = RepairOrdersStatus.Sent.ToString();
        repairOrder.CreatedAt = DateTime.Now;

        var availableVehicleParts = await _repairManagementApi.GetVehicleParts();
        if (availableVehicleParts == null || !availableVehicleParts.Any())
        {
            availableVehicleParts = new List<VehicleParts>();
        }

        var model = new RepairManagementNewViewModel
        {
            RepairOrder = repairOrder,
            AvailableVehicleParts = availableVehicleParts,
            SelectedVehicleParts = new List<Guid>()
        };

        return View(model);
    }

    public async Task<IActionResult> SendRepairOrder(RepairManagementNewViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return await HandleInvalidModelStateAsync(model);
        }

        var command = SendRepairOrderCommand(model);

        return await ExecuteSendRepairOrderAsync(model, command);
    }

    private async Task<IActionResult> HandleInvalidModelStateAsync(RepairManagementNewViewModel model)
    {
        _logger.LogError("Model state is invalid. Errors: {Errors}",
            string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))));

        model.AvailableVehicleParts = await _repairManagementApi.GetVehicleParts();
        return View("New", model);
    }

    private static SendRepairOrder SendRepairOrderCommand(RepairManagementNewViewModel model)
    {
        return new SendRepairOrder(
            messageId: Guid.NewGuid(),
            customerInfo: model.RepairOrder.CustomerInfo,
            vehicleInfo: model.RepairOrder.VehicleInfo,
            toRepairVehicleParts: model.SelectedVehicleParts,
            totalCost: model.RepairOrder.TotalCost,
            laborCost: model.RepairOrder.LaborCost,
            isApproved: false,
            createdAt: model.RepairOrder.CreatedAt,
            status: RepairOrdersStatus.Sent.ToString()
        );
    }

    private async Task<IActionResult> ExecuteSendRepairOrderAsync(RepairManagementNewViewModel model,
        SendRepairOrder command)
    {
        return await _resiliencyHelper.ExecuteResilient(async () =>
        {
            await _repairManagementApi.SendRepairOrder(command);
            _logger.LogInformation("Repair order created successfully for customer {Customer}",
                model.RepairOrder.CustomerInfo.CustomerName);
            return RedirectToAction("Index", "RepairManagement");
        }, View("Offline", new RepairManagementOfflineViewModel()));
    }
}