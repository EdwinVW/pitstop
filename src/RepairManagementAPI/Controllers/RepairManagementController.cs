using Pitstop.RepairManagementAPI.DTO;
using CustomerInfo = Pitstop.RepairManagementAPI.DTO.CustomerInfo;
using VehicleInfo = Pitstop.RepairManagementAPI.DTO.VehicleInfo;

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

        public async Task<IActionResult> GetRepairOrders()
        {
            try
            {
                var repairOrders = await _context.RepairOrders
                    .Select(ro => new
                    {
                        RepairOrderId = ro.Id,
                        ro.CutomerName,
                        ro.LicenseNumber,
                        ro.Status
                    })
                    .ToListAsync();

                var res = new List<RepairOrderDTO>();
                foreach (var repairOrder in repairOrders)
                {
                    var dto = new RepairOrderDTO
                    {
                        Id = repairOrder.RepairOrderId,
                        CustomerInfo = new CustomerInfo
                        {
                            CustomerName = repairOrder.CutomerName
                        },
                        VehicleInfo = new VehicleInfo
                        {
                            LicenseNumber = repairOrder.LicenseNumber
                        },
                        Status = repairOrder.Status
                    };
                    res.Add(dto);
                }

                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Ok();
            }
        }


        [HttpGet("{repairOrderId:guid}")]
        public async Task<IActionResult> GetByRepairOrderIdAsync(Guid repairOrderId)
        {
            var repairOrder = await _context.RepairOrders
                .Include(ro => ro.RepairOrderVehicleParts)
                .ThenInclude(rovp => rovp.VehicleParts)
                .FirstOrDefaultAsync(ro => ro.Id == repairOrderId);

            if (repairOrder == null)
            {
                return NotFound();
            }

            var result = new
            {
                repairOrder.Id,
                repairOrder.CutomerName,
                repairOrder.CustomerEmail,
                repairOrder.CustomerPhone,
                repairOrder.LicenseNumber,
                repairOrder.Type,
                repairOrder.Brand,
                repairOrder.TotalCost,
                repairOrder.LaborCost,
                repairOrder.IsApproved,
                repairOrder.CreatedAt,
                repairOrder.UpdatedAt,
                repairOrder.Status,
                repairOrder.RejectReason,
                VehicleParts = repairOrder.RepairOrderVehicleParts.Select(rovp => new
                {
                    PartName = rovp.VehicleParts.Name,
                    PartCost = rovp.VehicleParts.Cost
                })
            };

            return Ok(result);
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendRepairOrder([FromBody] SendRepairOrder command)
        {
            if (command == null || command.CustomerInfo == null || command.VehicleInfo == null)
            {
                Console.WriteLine($"Invalid request: SendRepairOrder command or its properties are null.");
                return BadRequest("Invalid request. Some required data is missing.");
            }

            Console.WriteLine($"SendRepairOrder Details:");
            Console.WriteLine($"  Customer: {command.CustomerInfo.CustomerName}");
            Console.WriteLine($"  LicenseNumber: {command.VehicleInfo.LicenseNumber}");
            Console.WriteLine($"  TotalCost: {command.TotalCost:C}");
            Console.WriteLine($"  LaborCost: {command.LaborCost:C}");
            Console.WriteLine($"  IsApproved: {command.IsApproved}");
            Console.WriteLine($"  CreatedAt: {command.CreatedAt}");
            Console.WriteLine($"  Status: {command.Status}");

            // Create new RepairOrder
            var repairOrder = new RepairOrder
            {
                Id = Guid.NewGuid(),
                CutomerName = command.CustomerInfo.CustomerName,
                CustomerEmail = command.CustomerInfo.CustomerEmail,
                CustomerPhone = command.CustomerInfo.CustomerPhone,
                LicenseNumber = command.VehicleInfo.LicenseNumber,
                Type = command.VehicleInfo.Type,
                Brand = command.VehicleInfo.Brand,
                TotalCost = command.TotalCost,
                LaborCost = command.LaborCost,
                IsApproved = command.IsApproved,
                CreatedAt = command.CreatedAt == DateTime.MinValue ? DateTime.Now : command.CreatedAt,
                UpdatedAt = DateTime.Now,
                Status = command.Status
            };

            await _context.RepairOrders.AddAsync(repairOrder);

            if (command.ToRepairVehicleParts != null)
            {
                foreach (var partId in command.ToRepairVehicleParts)
                {
                    var vehiclePart = await _context.VehicleParts.FindAsync(partId);
                    if (vehiclePart != null)
                    {
                        var repairOrderVehiclePart = new RepairOrderVehicleParts
                        {
                            RepairOrderId = repairOrder.Id,
                            VehiclePartsId = vehiclePart.Id
                        };
                        await _context.AddAsync(repairOrderVehiclePart);
                    }
                }
            }

            await _context.SaveChangesAsync();
            // var repairOrderSentEvent = new RepairOrderSent(
            //     command.MessageId,
            //     repairOrder.Id,
            //     command.CustomerInfo,
            //     command.VehicleInfo,
            //     command.ToRepairVehicleParts.Select(vp => (Name: _context.VehicleParts.First(v => v.Id == vp).Name,
            //         Cost: _context.VehicleParts.First(v => v.Id == vp).Cost)).ToList(),
            //     command.TotalCost,
            //     command.LaborCost,
            //     command.IsApproved,
            //     command.CreatedAt,
            //     command.Status
            // );
            //
            // await _messagePublisher.PublishMessageAsync(repairOrderSentEvent.MessageType, repairOrderSentEvent, "");

            return Ok(repairOrder);
        }


        [HttpPost("approve/{repairOrderId:guid}")]
        public async Task<IActionResult> ApproveRepairOrder(Guid repairOrderId)
        {
            var repairOrder = await _context.RepairOrders.FirstOrDefaultAsync(x => x.Id == repairOrderId);
            if (repairOrder == null)
            {
                return NotFound();
            }


            repairOrder.Status = RepairOrdersStatus.Approved.ToString();
            repairOrder.UpdatedAt = DateTime.Now;
            repairOrder.IsApproved = true;
            await _context.SaveChangesAsync();


            // var e = RepairOrderApproved.FromCommand(command);
            // await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

            return Ok(repairOrder);
        }


        [HttpPost("reject/{repairOrderId:guid}")]
        public async Task<IActionResult> RejectRepairOrder(Guid repairOrderId,
            [FromBody] RejectRepairOrderReason rejectReason)
        {
            var repairOrder = await _context.RepairOrders.FirstOrDefaultAsync(x => x.Id == repairOrderId);
            if (repairOrder == null)
            {
                return NotFound();
            }


            repairOrder.Status = RepairOrdersStatus.Rejected.ToString();
            repairOrder.UpdatedAt = DateTime.Now;
            repairOrder.IsApproved = false;
            repairOrder.RejectReason = rejectReason.RejectReason;

            await _context.SaveChangesAsync();

            //
            // var e = RepairOrderRejected.FromCommand(command);
            // await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

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