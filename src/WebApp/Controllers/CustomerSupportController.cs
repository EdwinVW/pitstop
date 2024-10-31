namespace PitStop.WebApp.Controllers;

public class CustomerSupportController : Controller
{
    private readonly ICustomerSupportAPI _customerSupportApi;   
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    private readonly ResiliencyHelper _resiliencyHelper;
    
    public CustomerSupportController(ICustomerSupportAPI customerSupportApi, ILogger<CustomerSupportController> logger)
    {
        _customerSupportApi = customerSupportApi;
        _logger = logger;
        _resiliencyHelper = new ResiliencyHelper(_logger);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return await _resiliencyHelper.ExecuteResilient(async () =>
        {
            var model = new CustomerSupportViewModel
            {
                Rejections = await _customerSupportApi.GetRejections()
            };

            return await Task.FromResult<IActionResult>(View(model));
        }, View("Offline", new CustomerSupportOfflineViewModel()));
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }
}