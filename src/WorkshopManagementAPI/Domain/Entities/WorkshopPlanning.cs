namespace Pitstop.WorkshopManagementAPI.Domain.Entities;

public class WorkshopPlanning : AggregateRoot<WorkshopPlanningId>
{
    /// <summary>
    /// The list of maintenance-jobs for this day. 
    /// </summary>
    public List<MaintenanceJob> Jobs { get; private set; }

    public WorkshopPlanning(DateTime date) : base(WorkshopPlanningId.Create(date)) { }

    public WorkshopPlanning(DateTime date, IEnumerable<Event> events) : base(WorkshopPlanningId.Create(date), events) { }

    /// <summary>
    /// Creates a new instance of a workshop-planning for the specified date.
    /// </summary>
    /// <param name="date">The date to create the planning for.</param>
    public static WorkshopPlanning Create(DateTime date)
    {
        WorkshopPlanning planning = new WorkshopPlanning(date);
        WorkshopPlanningCreated e = new WorkshopPlanningCreated(Guid.NewGuid(), date);
        planning.RaiseEvent(e);
        return planning;
    }

    public void PlanMaintenanceJob(PlanMaintenanceJob command)
    {
        // check business rules
        command.PlannedMaintenanceJobShouldFallWithinOneBusinessDay();
        this.NumberOfParallelMaintenanceJobsMustNotExceedAvailableWorkStations(command);
        this.NumberOfParallelMaintenanceJobsOnAVehicleMustNotExceedOne(command);

        // handle event
        MaintenanceJobPlanned e = command.MapToMaintenanceJobPlanned();
        RaiseEvent(e);
    }

    public void FinishMaintenanceJob(FinishMaintenanceJob command)
    {
        // find job
        MaintenanceJob job = Jobs.FirstOrDefault(j => j.Id == command.JobId);
        if (job == null)
        {
            throw new MaintenanceJobNotFoundException($"Maintenance job with id {command.JobId} found.");
        }

        // check business rules
        job.FinishedMaintenanceJobCanNotBeFinished();

        // handle event
        MaintenanceJobFinished e = command.MapToMaintenanceJobFinished();
        RaiseEvent(e);
    }

    /// <summary>
    /// Handles an event and updates the aggregate version.
    /// </summary>
    /// <remarks>Caution: this handles is also called while replaying events to restore state.
    /// So, do not execute any checks that could fail or introduce any side-effects in this handler.</remarks>
    protected override void When(dynamic @event)
    {
        Handle(@event);
    }

    private void Handle(WorkshopPlanningCreated e)
    {
        Jobs = new List<MaintenanceJob>();
    }

    private void Handle(MaintenanceJobPlanned e)
    {
        MaintenanceJob job = new MaintenanceJob(e.JobId);
        Customer customer = new Customer(e.CustomerInfo.Id, e.CustomerInfo.Name, e.CustomerInfo.TelephoneNumber);
        LicenseNumber licenseNumber = LicenseNumber.Create(e.VehicleInfo.LicenseNumber);
        Vehicle vehicle = new Vehicle(licenseNumber, e.VehicleInfo.Brand, e.VehicleInfo.Type, customer.Id);
        Timeslot plannedTimeslot = Timeslot.Create(e.StartTime, e.EndTime);
        job.Plan(plannedTimeslot, vehicle, customer, e.Description);
        Jobs.Add(job);
    }

    private void Handle(MaintenanceJobFinished e)
    {
        MaintenanceJob job = Jobs.FirstOrDefault(j => j.Id == e.JobId);
        Timeslot actualTimeslot = Timeslot.Create(e.StartTime, e.EndTime);
        job.Finish(actualTimeslot, e.Notes);
    }
}