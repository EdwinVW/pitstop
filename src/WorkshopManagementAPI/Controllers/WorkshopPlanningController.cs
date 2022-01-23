namespace Pitstop.WorkshopManagementAPI.Controllers;

[Route("/api/[controller]")]
public class WorkshopPlanningController : Controller
{
    private readonly IEventSourceRepository<WorkshopPlanning> _planningRepo;
    private readonly IPlanMaintenanceJobCommandHandler _planMaintenanceJobCommandHandler;
    private readonly IFinishMaintenanceJobCommandHandler _finishMaintenanceJobCommandHandler;

    public WorkshopPlanningController(
        IEventSourceRepository<WorkshopPlanning> planningRepo,
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
            var aggregateId = WorkshopPlanningId.Create(planningDate);
            var planning = await _planningRepo.GetByIdAsync(aggregateId);
            if (planning == null)
            {
                return NotFound();
            }

            return Ok(planning.MapToDTO());
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
                var aggregateId = WorkshopPlanningId.Create(planningDate);
                var planning = await _planningRepo.GetByIdAsync(aggregateId);
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
                return Ok(job.MapToDTO());
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
                    return CreatedAtRoute("GetByDate", new { planningDate = planning.Id }, planning.MapToDTO());
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