namespace Pitstop.WorkshopManagementAPI.Domain.Entities;

public class MaintenanceJob : Entity<Guid>
{
    public Timeslot PlannedTimeslot { get; private set; }
    public Vehicle Vehicle { get; private set; }
    public Customer Customer { get; private set; }
    public string Description { get; private set; }
    public Timeslot ActualTimeslot { get; private set; }
    public string Notes { get; private set; }
    public string Status => (ActualTimeslot == null) ? "Planned" : "Completed";

    public MaintenanceJob(Guid id) : base(id)
    {

    }

    public void Plan(Timeslot timeslot, Vehicle vehicle, Customer customer, string description)
    {
        PlannedTimeslot = timeslot;
        Vehicle = vehicle;
        Customer = customer;
        Description = description;
    }

    public void Finish(Timeslot actualTimeslot, string notes)
    {

        ActualTimeslot = actualTimeslot;
        Notes = notes;
    }

}