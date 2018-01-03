using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitstop.Application.VehicleManagement.Model;
using Pitstop.Application.VehicleManagement.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Pitstop.Infrastructure.Messaging;
using Pitstop.Application.VehicleManagement.Events;
using Pitstop.Application.VehicleManagement.Commands;

namespace Pitstop.Application.VehicleManagement.Controllers
{
    [Route("/api/[controller]")]
    public class VehiclesController : Controller
    {
        IMessagePublisher _messagePublisher;
        VehicleManagementDBContext _dbContext;

        public VehiclesController(VehicleManagementDBContext dbContext, IMessagePublisher messagePublisher)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _dbContext.Vehicles.ToListAsync());
        }

        [HttpGet]
        [Route("{licenseNumber}", Name = "GetByLicenseNumber")]
        public async Task<IActionResult> GetByLicenseNumber(string licenseNumber)
        {
            var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.LicenseNumber == licenseNumber);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterVehicle command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // insert vehicle
                    Vehicle vehicle = Mapper.Map<Vehicle>(command);
                    _dbContext.Vehicles.Add(vehicle);
                    await _dbContext.SaveChangesAsync();

                    // send event
                    var e = Mapper.Map<VehicleRegistered>(command);
                    await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

                    //return result
                    return CreatedAtRoute("GetByLicenseNumber", new { licenseNumber = vehicle.LicenseNumber }, vehicle);
                }
                return BadRequest();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
