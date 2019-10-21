using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Pitstop.WorkshopManagementAPI.Repositories;
using System;
using Pitstop.WorkshopManagementAPI.Domain;
using System.Linq;
using Pitstop.WorkshopManagementAPI.Commands;
using Pitstop.WorkshopManagementAPI.Domain.Exceptions;
using Pitstop.WorkshopManagementAPI.Models;
using WorkshopManagementAPI.CommandHandlers;
using Serilog;
using System.Globalization;

namespace Pitstop.WorkshopManagementAPI.Controllers
{
    [Route("/api/[controller]")]
    public class WorkshopPlanningController : Controller
    {
        private readonly IWorkshopPlanningRepository _planningRepo;
        private readonly IPlanMaintenanceJobCommandHandler _planMaintenanceJobCommandHandler;
        private readonly IFinishMaintenanceJobCommandHandler _finishMaintenanceJobCommandHandler;

        public WorkshopPlanningController(
            IWorkshopPlanningRepository planningRepo,
            IPlanMaintenanceJobCommandHandler planMaintenanceJobCommandHandler,
            IFinishMaintenanceJobCommandHandler finishMaintenanceJobCommand)
        {
            _planningRepo = planningRepo;
            _planMaintenanceJobCommandHandler = planMaintenanceJobCommandHandler;
            _finishMaintenanceJobCommandHandler = finishMaintenanceJobCommand;
        }

        [HttpGet]
        [Route("{planningDate}", Name = "GetByDate")]
        public async Task<IActionResult> GetByDate(DateTime planningDate)
        {
            try
            {
                var planning = await _planningRepo.GetWorkshopPlanningAsync(planningDate);
                if (planning == null)
                {
                    return NotFound();
                }

                return Ok(planning);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
        }

        [HttpGet]
        [Route("{planningDate}/jobs/{jobId}")]
        public async Task<IActionResult> GetMaintenanceJobAsync(DateTime planningDate, Guid jobId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // get planning
                    WorkshopPlanning planning = await _planningRepo.GetWorkshopPlanningAsync(planningDate);
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
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    throw;
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("{planningDate}/jobs")]
        public async Task<IActionResult> PlanMaintenanceJobAsync(DateTime planningDate, [FromBody] PlanMaintenanceJob command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // handle command
                        WorkshopPlanning planning = await
                            _planMaintenanceJobCommandHandler.HandleCommandAsync(planningDate, command);

                        // handle result    
                        if (planning == null)
                        {
                            return NotFound();
                        }

                        // return result
                        return CreatedAtRoute("GetByDate", new { planningDate = planning.Id }, planning);
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
                string errorMessage = "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.";
                Log.Error(errorMessage);
                ModelState.AddModelError("ErrorMessage", errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("{planningDate}/jobs/{jobId}/finish")]
        public async Task<IActionResult> FinishMaintenanceJobAsync(DateTime planningDate, Guid jobId, [FromBody] FinishMaintenanceJob command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // handle command
                    WorkshopPlanning planning = await
                        _finishMaintenanceJobCommandHandler.HandleCommandAsync(planningDate, command);

                    // handle result    
                    if (planning == null)
                    {
                        return NotFound();
                    }

                    // return result
                    return Ok();
                }
                return BadRequest();
            }
            catch (ConcurrencyException)
            {
                string errorMessage = "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.";
                Log.Error(errorMessage);
                ModelState.AddModelError("", errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
