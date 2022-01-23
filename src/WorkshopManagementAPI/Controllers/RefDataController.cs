namespace Pitstop.WorkshopManagementAPI.Controllers;

[Route("/api/[controller]")]
public class RefDataController : Controller
{
    ICustomerRepository _customerRepo;
    IVehicleRepository _vehicleRepo;

    public RefDataController(ICustomerRepository customerRepo, IVehicleRepository vehicleRepo)
    {
        _customerRepo = customerRepo;
        _vehicleRepo = vehicleRepo;
    }

    [HttpGet]
    [Route("customers")]
    public async Task<IActionResult> GetCustomers()
    {
        return Ok(await _customerRepo.GetCustomersAsync());
    }

    [HttpGet]
    [Route("customers/{customerId}")]
    public async Task<IActionResult> GetCustomerByCustomerId(string customerId)
    {
        var customer = await _customerRepo.GetCustomerAsync(customerId);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpGet]
    [Route("vehicles")]
    public async Task<IActionResult> GetVehicles()
    {
        return Ok(await _vehicleRepo.GetVehiclesAsync());
    }

    [HttpGet]
    [Route("vehicles/{licenseNumber}")]
    public async Task<IActionResult> GetVehicleByLicenseNumber(string licenseNumber)
    {
        var vehicle = await _vehicleRepo.GetVehicleAsync(licenseNumber);
        if (vehicle == null)
        {
            return NotFound();
        }
        return Ok(vehicle);
    }
}