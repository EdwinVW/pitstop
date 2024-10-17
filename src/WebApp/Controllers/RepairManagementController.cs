namespace PitStop.WebApp.Controllers;

public class RepairManagementController : Controller
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    private ResiliencyHelper _resiliencyHelper;

    public RepairManagementController(ILogger<RepairManagementController> logger)
    {
        _logger = logger;
        _resiliencyHelper = new ResiliencyHelper(_logger);
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }
}