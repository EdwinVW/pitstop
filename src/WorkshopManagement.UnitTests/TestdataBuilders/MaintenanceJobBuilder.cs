namespace Pitstop.WorkshopManagement.UnitTests.TestdataBuilders;

public class MaintenanceJobBuilder
{
    public Guid JobId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string Description { get; private set; }
    public string Status { get; private set; }
    public CustomerBuilder CustomerBuilder { get; private set; }
    public VehicleBuilder VehicleBuilder { get; private set; }

    public MaintenanceJobBuilder()
    {
        SetDefaults();
    }

    public MaintenanceJobBuilder WithJobId(Guid id)
    {
        JobId = id;
        return this;
    }

    public MaintenanceJobBuilder WithStartTime(DateTime startTime)
    {
        StartTime = startTime;
        return this;
    }

    public MaintenanceJobBuilder WithEndTime(DateTime endTime)
    {
        EndTime = endTime;
        return this;
    }

    public MaintenanceJobBuilder WithStatus(string status)
    {
        Status = status;
        return this;
    }

    public MaintenanceJobBuilder WithCustomer(Customer customer)
    {
        CustomerBuilder
            .WithId(customer.Id)
            .WithName(customer.Name)
            .WithTelephoneNumber(customer.TelephoneNumber);

        VehicleBuilder.WithOwnerId(customer.Id);

        return this;
    }

    public MaintenanceJobBuilder WithVehicle(Vehicle vehicle)
    {
        VehicleBuilder
            .WithLicenseNumber(vehicle.Id)
            .WithBrand(vehicle.Brand)
            .WithType(vehicle.Type)
            .WithOwnerId(vehicle.OwnerId);

        return this;
    }

    public MaintenanceJob Build()
    {
        if (CustomerBuilder == null)
        {
            throw new InvalidOperationException("You must specify a customerbuilder using the 'WithCustomerBuilder' method.");
        }

        if (VehicleBuilder == null)
        {
            throw new InvalidOperationException("You must specify a vehiclebuilder using the 'WithVehicleBuilder' method.");
        }

        Customer customer = CustomerBuilder.Build();
        Vehicle vehicle = VehicleBuilder.Build();

        var job = new MaintenanceJob(JobId);
        Timeslot plannedTimeslot = Timeslot.Create(StartTime, EndTime);
        job.Plan(plannedTimeslot, vehicle, customer, Description);
        return job;
    }

    private void SetDefaults()
    {
        JobId = Guid.NewGuid();
        StartTime = DateTime.Today.AddHours(8);
        EndTime = DateTime.Today.AddHours(11);
        Description = $"Job {JobId}";
        CustomerBuilder = new CustomerBuilder();
        VehicleBuilder = new VehicleBuilder();
        VehicleBuilder.WithOwnerId(CustomerBuilder.Id);
    }
}