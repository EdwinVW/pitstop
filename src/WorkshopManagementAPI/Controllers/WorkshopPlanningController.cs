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
using WorkshopManagementAPI.CommandHandlers;
using Serilog;
using WorkshopManagementAPI.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Pitstop.WorkshopManagementAPI.Controllers
{
    [Route("/api/[controller]")]
    public class WorkshopPlanningController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWorkshopPlanningRepository _planningRepo;

        public WorkshopPlanningController(IServiceProvider serviceProvider, 
            IWorkshopPlanningRepository planningRepo)
        {
            _serviceProvider = serviceProvider;
            _planningRepo = planningRepo;
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

        [HttpPost]
        [Route("{planningDate}")]
        public async Task<IActionResult> RegisterPlanningAsync(DateTime planningDate, [FromBody] RegisterPlanning cmd)
        {
            try
            {
                // handle command
                WorkshopPlanning planning = await 
                    _serviceProvider.GetRequiredService<IRegisterPlanningCommandHandler>()
                    .HandleCommandAsync(planningDate, cmd);

                // return result
                return CreatedAtRoute("GetByDate", new { planningDate = planning.Date }, planning);
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
                            _serviceProvider.GetRequiredService<IPlanMaintenanceJobCommandHandler>()
                            .HandleCommandAsync(planningDate, command);

                        // handle result    
                        if (planning == null)
                        {
                            return NotFound();
                        }

                        // return result
                        return CreatedAtRoute("GetByDate", new { planningDate = planning.Date }, planning);
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
                            _serviceProvider.GetRequiredService<IFinishMaintenanceJobCommandHandler>()                        
                            .HandleCommandAsync(planningDate, command);

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
