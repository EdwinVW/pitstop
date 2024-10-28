namespace Pitstop.CustomerSupportAPI.Controllers;

public class CustomerSupportController : ControllerBase
{
    private readonly CustomerSupportContext _context;
    private readonly IMessagePublisher _messagePublisher;

    public CustomerSupportController(CustomerSupportContext context, IMessagePublisher messagePublisher)
    {
        _context = context;
        _messagePublisher = messagePublisher;
    }
}