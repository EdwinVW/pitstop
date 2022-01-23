namespace Pitstop.WorkshopManagement.UnitTests.TestdataBuilders;

public class PlanMaintenanceJobCommandBuilder
{
    public MaintenanceJobBuilder MaintenanceJobBuilder { get; private set; }
    public CustomerBuilder CustomerBuilder { get; private set; }
    public VehicleBuilder VehicleBuilder { get; private set; }

    public PlanMaintenanceJobCommandBuilder()
    {
        SetDefaults();
    }

    public PlanMaintenanceJobCommandBuilder WithMaintenanceJobBuilder(MaintenanceJobBuilder maintenanceJobBuilder)
    {
        MaintenanceJobBuilder = maintenanceJobBuilder;
        return this;
    }

    public PlanMaintenanceJobCommandBuilder WithVehicleBuilder(VehicleBuilder vehicleBuilder)
    {
        VehicleBuilder = vehicleBuilder;
        return this;
    }

    public PlanMaintenanceJob Build()
    {
        var customer = CustomerBuilder
           .Build();

        var vehicle = VehicleBuilder
            .WithOwnerId(customer.Id)
            .Build();

        var job = MaintenanceJobBuilder
            .WithCustomer(customer)
            .WithVehicle(vehicle)
            .Build();

        PlanMaintenanceJob command = new PlanMaintenanceJob(
            Guid.NewGuid(), job.Id, job.PlannedTimeslot.StartTime, job.PlannedTimeslot.EndTime,
            (customer.Id, customer.Name, customer.TelephoneNumber),
            (vehicle.Id, vehicle.Brand, vehicle.Type),
            job.Description
        );

        return command;
    }

    private void SetDefaults()
    {
        CustomerBuilder = new CustomerBuilder();
        VehicleBuilder = new VehicleBuilder();
        MaintenanceJobBuilder = new MaintenanceJobBuilder();
    }
}