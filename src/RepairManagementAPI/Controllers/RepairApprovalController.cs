namespace Pitstop.RepairManagementAPI.Controllers
{
    [Route("api/[controller]")]
    public class RepairManagementController : ControllerBase
    {
        private readonly RepairManagementContext _context;
        private readonly IMessagePublisher _messagePublisher;

        public RepairManagementController(RepairManagementContext context, IMessagePublisher messagePublisher)
        {
            _context = context;
            _messagePublisher = messagePublisher;
        }

        // Get all repair orders
        [HttpGet("repairorders")]
        public async Task<IActionResult> GetRepairOrders()
        {
            var repairOrders = await _context.RepairOrders.ToListAsync();
            return Ok(repairOrders);
        }

        // Get a specific repair order by its ID
        [HttpGet("repairorders/{repairOrderId:guid}")]
        public async Task<IActionResult> GetByRepairOrderIdAsync(Guid repairOrderId)
        {
            var repairOrder = await _context.RepairOrders.FindAsync(repairOrderId);
            if (repairOrder == null)
            {
                return NotFound();
            }

            return Ok(repairOrder);
        }

        // Create a new repair order
        [HttpPost("repairorders/create")]
        public async Task<IActionResult> CreateRepairOrder([FromBody] CreateRepairOrder command)
        {
            // Map the command to a RepairOrders entity
            RepairOrders repairOrder = command.MapToRepairOrders();
            repairOrder.Status = RepairOrdersStatus.Sent;
            repairOrder.CreatedAt = DateTime.Now;
            repairOrder.UpdatedAt = DateTime.Now;

            await _context.RepairOrders.AddAsync(repairOrder);
            await _context.SaveChangesAsync();

            // Create and send the RepairOrderCreated event
            var e = RepairOrderCreated.FromCommand(command);
            await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

            return Ok(repairOrder);
        }

        // Approve a repair order
        [HttpPost("approve/{repairOrderId:guid}")]
        public async Task<IActionResult> ApproveRepairOrder(Guid repairOrderId)
        {
            var repairOrder = await _context.RepairOrders.FirstOrDefaultAsync(x => x.Id == repairOrderId);
            if (repairOrder == null)
            {
                return NotFound();
            }

            // Approve the repair order
            repairOrder.Status = RepairOrdersStatus.Approved;
            repairOrder.UpdatedAt = DateTime.Now;
            repairOrder.IsApproved = true;
            await _context.SaveChangesAsync();

            // Create and send the RepairOrderApproved event
            var e = RepairOrderApproved.FromCommand(repairOrderId.ToString());
            await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

            return Ok(repairOrder);
        }

        // Reject a repair order
        [HttpPost("reject/{repairOrderId:guid}")]
        public async Task<IActionResult> RejectRepairOrder(Guid repairOrderId, [FromBody] RejectRepairOrder command)
        {
            var repairOrder = await _context.RepairOrders.FirstOrDefaultAsync(x => x.Id == repairOrderId);
            if (repairOrder == null)
            {
                return NotFound();
            }

            // Reject the repair order
            repairOrder.Status = RepairOrdersStatus.Rejected;
            repairOrder.UpdatedAt = DateTime.Now;
            repairOrder.IsApproved = false;
            repairOrder.RejectReason = command.RejectReason;

            await _context.SaveChangesAsync();

            // Create and send the RepairOrderRejected event
            var e = RepairOrderRejected.FromCommand(command);
            await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

            return Ok(repairOrder);
        }

        // Get all vehicle parts
        [HttpGet("vehicleparts")]
        public async Task<IActionResult> GetVehicleParts()
        {
            var vehicleParts = await _context.VehicleParts.ToListAsync();
            return Ok(vehicleParts);
        }
    }
}