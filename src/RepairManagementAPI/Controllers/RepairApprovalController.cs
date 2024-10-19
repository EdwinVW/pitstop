using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pitstop.Infrastructure.Messaging;
using Pitstop.RepairManagementAPI.DataAccess;
using Pitstop.RepairManagementAPI.Model; // Assuming RepairOrder is located here
using System.Threading.Tasks;
using Pitstop.RepairManagementAPI.Commands;
using Pitstop.RepairManagementAPI.Enums;
using Pitstop.RepairManagementAPI.Events;
using Pitstop.RepairManagementAPI.Mappers;

namespace Pitstop.RepairManagementAPI.Controllers
{
    [Route("api/[controller]")]
    public class RepairManagementController : ControllerBase
    {
        private readonly RepairManagementApiDBContext _dbContext;
        private readonly IMessagePublisher _messagePublisher;

        public RepairManagementController(RepairManagementApiDBContext dbContext, IMessagePublisher messagePublisher)
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

        // Send message with the created repair order
        [HttpPost("create")]
        public async Task<IActionResult> CreateRepairOrder([FromBody] CreateRepairOrder command)
        {
            // Add repair order to the database
            RepairOrders repairOrder = command.MapToRepairOrders();
            await _dbContext.RepairOrders.AddAsync(repairOrder);
            await _dbContext.SaveChangesAsync();

            // Send message after creation
            var e = RepairOrderCreated.FromCommand(command);
            await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

            return Ok(command);
        }

        // Approve a repair order
        [HttpPost("{repairOrderId}/approve")]
        public async Task<IActionResult> ApproveRepair([FromBody] ApproveRepairOrder command)
        {
            var repairOrder = await _dbContext.RepairOrders.FirstOrDefaultAsync(x => x.Id == command.RepairOrderId);
            if (repairOrder == null)
            {
                return NotFound();
            }

            // Logic to approve the repair order (e.g., updating status)
            repairOrder.Status = RepairOrdersStatus.Approved; // Example status update
            repairOrder.UpdatedAt = DateTime.Now;
            repairOrder.IsApproved = true;
            await _dbContext.SaveChangesAsync();

            var e = RepairOrderApproved.FromCommand(command);
            // Send approval message
            await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

            return Ok(repairOrder);
        }

        // Reject a repair order
        [HttpPost("{repairOrderId}/reject")]
        public async Task<IActionResult> RejectRepair([FromBody] RejectRepairOrder command)
        {
            var repairOrder = await _dbContext.RepairOrders.FirstOrDefaultAsync(x => x.Id == command.RepairOrderId);
            if (repairOrder == null)
            {
                return NotFound();
            }

            // Logic to reject the repair order (e.g., updating status)
            repairOrder.Status = RepairOrdersStatus.Rejected; // Example status update
            repairOrder.UpdatedAt = DateTime.Now;
            repairOrder.IsApproved = false;
            repairOrder.RejectReason = command.RejectReason;

            await _dbContext.SaveChangesAsync();

            var e = RepairOrderRejected.FromCommand(command);

            // Send rejection message
            await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

            return Ok(repairOrder);
        }
    }
}