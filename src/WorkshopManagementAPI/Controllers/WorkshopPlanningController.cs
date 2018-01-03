using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Pitstop.Infrastructure.Messaging;
using Pitstop.WorkshopManagementAPI.Repositories;
using System;
using Pitstop.WorkshopManagementAPI.Domain;
using System.Collections.Generic;
using System.Linq;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain.Exceptions;
using Pitstop.WorkshopManagementAPI.Models;

namespace Pitstop.WorkshopManagementAPI.Controllers
{
    [Route("/api/[controller]")]
    public class WorkshopPlanningController : Controller
    {
        IMessagePublisher _messagePublisher;
        IWorkshopPlanningRepository _planningRepo;

        public WorkshopPlanningController(IWorkshopPlanningRepository planningRepo, IMessagePublisher messagePublisher)
        {
            _planningRepo = planningRepo;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        [Route("{date}", Name = "GetByDate")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var planning = await _planningRepo.GetWorkshopPlanningAsync(date);
            if (planning == null)
            {
                return NotFound();
            }
            return Ok(planning);
        }

        [HttpPost]
        [Route("{date}")]
        public async Task<IActionResult> RegisterAsync(DateTime date)
        {
            try
            {
                // insert planning
                WorkshopPlanning planning = new WorkshopPlanning();
                IEnumerable<Event> events = planning.Create(date);
                await _planningRepo.SaveWorkshopPlanningAsync(planning, events);

                // return result
                return CreatedAtRoute("GetByDate", new { date = planning.Date }, planning);
            }
            catch (ConcurrencyException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{date}/jobs/{jobId}")]
        public async Task<IActionResult> GetMaintenanceJobAsync(DateTime date, Guid jobId)
        {
            if (ModelState.IsValid)
            {
                // get planning
                WorkshopPlanning planning = await _planningRepo.GetWorkshopPlanningAsync(date);
                if (planning == null || planning.Jobs == null)
                {
                    return NotFound();
                }
                // get job
                var job = planning.Jobs.FirstOrDefault(j => j.Id == jobId);
                if (job == null)
                {
                    return NotFound();
                }
                return Ok(job);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("{date}/jobs")]
        public async Task<IActionResult> PlanMaintenanceJobAsync(DateTime date, [FromBody] PlanMaintenanceJob command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // get planning
                    WorkshopPlanning planning = await _planningRepo.GetWorkshopPlanningAsync(date);
                    if (planning == null)
                    {
                        return NotFound();
                    }

                    // handle command
                    try
                    {
                        IEnumerable<Event> events = planning.PlanMaintenanceJob(command);

                        // persist
                        await _planningRepo.SaveWorkshopPlanningAsync(planning, events);

                        // publish event
                        foreach (var e in events)
                        {
                            await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
                        }

                        // return result
                        return CreatedAtRoute("GetByDate", new { date = planning.Date }, planning);
                    }
                    catch (BusinessRuleViolationException ex)
                    {
                        return StatusCode(StatusCodes.Status409Conflict, new BusinessRuleViolation { ErrorMessage = ex.Message });
                    }
                }
                return BadRequest();
            }
            catch (ConcurrencyException)
            {
                ModelState.AddModelError("ErrorMessage", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("{date}/jobs/{jobId}/finish")]
        public async Task<IActionResult> FinishMaintenanceJobAsync(DateTime date, Guid jobId, [FromBody] FinishMaintenanceJob command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // get planning
                    WorkshopPlanning planning = await _planningRepo.GetWorkshopPlanningAsync(date);
                    if (planning == null)
                    {
                        return NotFound();
                    }

                    // handle command
                    IEnumerable<Event> events = planning.FinishMaintenanceJob(command);

                    // persist
                    await _planningRepo.SaveWorkshopPlanningAsync(planning, events);

                    // publish event
                    foreach (var e in events)
                    {
                        await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
                    }

                    // return result
                    return Ok();
                }
                return BadRequest();
            }
            catch (ConcurrencyException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
