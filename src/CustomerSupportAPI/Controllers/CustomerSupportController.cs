namespace Pitstop.CustomerSupportAPI.Controllers;

[Route("api/[controller]")]
public class CustomerSupportController : Controller
{
    private readonly CustomerSupportContext _context;
    private readonly IMessagePublisher _messagePublisher;

    public CustomerSupportController(CustomerSupportContext context, IMessagePublisher messagePublisher)
    {
        _context = context;
        _messagePublisher = messagePublisher;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _context.Rejections.ToListAsync());
    }
}