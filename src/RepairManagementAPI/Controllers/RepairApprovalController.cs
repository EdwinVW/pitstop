namespace Pitstop.RepairManagementAPI.Controllers
{
    [Route("api/[controller]")]
    public class RepairManagementController : ControllerBase
    {
        private readonly RepairManagementDBContext _dbContext;
        private readonly IMessagePublisher _messagePublisher;

        public RepairManagementController(RepairManagementDBContext dbContext, IMessagePublisher messagePublisher)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
        }

        // Get all repair orders
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var repairOrders = await _dbContext.RepairOrders.ToListAsync();
            return Ok(repairOrders);
        }

        // Get a specific repair order by its ID
        [HttpGet]
        [Route("{repairOrderId:guid}", Name = "GetByRepairOrderId")]
        public async Task<IActionResult> GetByRepairOrderIdAsync(Guid repairOrderId)
        {
            var repairOrder = await _dbContext.RepairOrders.FindAsync(repairOrderId);
            if (repairOrder == null)
            {
                return NotFound();
            }

            return Ok(repairOrder);
        }

        // Create a new repair order
        [HttpPost("create")]
        public async Task<IActionResult> CreateRepairOrder([FromBody] CreateRepairOrder command)
        {
            // Map the command to a RepairOrders entity
            RepairOrders repairOrder = command.MapToRepairOrders();
            repairOrder.Status = RepairOrdersStatus.Sent;
            repairOrder.CreatedAt = DateTime.Now;
            repairOrder.UpdatedAt = DateTime.Now;

            await _dbContext.RepairOrders.AddAsync(repairOrder);
            await _dbContext.SaveChangesAsync();

            // Create and send the RepairOrderCreated event
            var e = RepairOrderCreated.FromCommand(command);
            await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

            return Ok(repairOrder);
        }

        // Approve a repair order
        [HttpPost]
        [Route("/approve{repairOrderId:guid}", Name = "ApproveRepairOrder")]
        public async Task<IActionResult> ApproveRepairOrder(Guid repairOrderId)
        {
            var repairOrder = await _dbContext.RepairOrders.FirstOrDefaultAsync(x => x.Id == repairOrderId);
            if (repairOrder == null)
            {
                return NotFound();
            }

            // Approve the repair order
            repairOrder.Status = RepairOrdersStatus.Approved;
            repairOrder.UpdatedAt = DateTime.Now;
            repairOrder.IsApproved = true;
            await _dbContext.SaveChangesAsync();

            // Create and send the RepairOrderApproved event
            var e = RepairOrderApproved.FromCommand(repairOrderId.ToString());
            await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

            return Ok(repairOrder);
        }

        // Reject a repair order
        [HttpPost]
        [Route("/reject{repairOrderId:guid}", Name = "RejectRepairOrder")]
        public async Task<IActionResult> RejectRepairOrder(Guid repairOrderId, [FromBody] RejectRepairOrder command)
        {
            var repairOrder = await _dbContext.RepairOrders.FirstOrDefaultAsync(x => x.Id == repairOrderId);
            if (repairOrder == null)
            {
                return NotFound();
            }

            // Reject the repair order
            repairOrder.Status = RepairOrdersStatus.Rejected;
            repairOrder.UpdatedAt = DateTime.Now;
            repairOrder.IsApproved = false;
            repairOrder.RejectReason = command.RejectReason;

            await _dbContext.SaveChangesAsync();

            // Create and send the RepairOrderRejected event
            var e = RepairOrderRejected.FromCommand(command);
            await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

            return Ok(repairOrder);
        }
    }
}
