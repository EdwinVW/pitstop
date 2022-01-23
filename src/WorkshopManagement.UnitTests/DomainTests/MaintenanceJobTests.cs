namespace WorkshopManagement.UnitTests.DomainTests;

public class MaintenanceJobTests
{
    [Fact]
    public void Plan_Should_Create_A_New_Job()
    {
        // arrange
        MaintenanceJobBuilder maintenanceJobBuilder = new MaintenanceJobBuilder();
        MaintenanceJob sut = maintenanceJobBuilder
            .Build();

        // act
        // sut.Plan() is called by the Testdata Builder

        // assert
        Assert.Equal(maintenanceJobBuilder.JobId, sut.Id);
        Assert.Equal(maintenanceJobBuilder.StartTime, sut.PlannedTimeslot.StartTime);
        Assert.Equal(maintenanceJobBuilder.EndTime, sut.PlannedTimeslot.EndTime);
        Assert.Equal(maintenanceJobBuilder.CustomerBuilder.Id, sut.Customer.Id);
        Assert.Equal(maintenanceJobBuilder.CustomerBuilder.Name, sut.Customer.Name);
        Assert.Equal(maintenanceJobBuilder.CustomerBuilder.TelephoneNumber, sut.Customer.TelephoneNumber);
        Assert.Equal(maintenanceJobBuilder.VehicleBuilder.LicenseNumber, sut.Vehicle.Id);
        Assert.Equal(maintenanceJobBuilder.VehicleBuilder.Brand, sut.Vehicle.Brand);
        Assert.Equal(maintenanceJobBuilder.VehicleBuilder.Type, sut.Vehicle.Type);
        Assert.Equal(maintenanceJobBuilder.VehicleBuilder.OwnerId, sut.Vehicle.OwnerId);
        Assert.Equal(maintenanceJobBuilder.Description, sut.Description);
        Assert.Null(sut.ActualTimeslot);
        Assert.Null(sut.Notes);
        Assert.Equal("Planned", sut.Status);
    }

    [Fact]
    public void Finish_Should_Finish_An_Existing_Job()
    {
        // arrange
        MaintenanceJobBuilder maintenanceJobBuilder = new MaintenanceJobBuilder();
        MaintenanceJob sut = maintenanceJobBuilder
            .Build();

        DateTime actualStartTime = maintenanceJobBuilder.StartTime.AddMinutes(30);
        DateTime actualEndTime = maintenanceJobBuilder.EndTime.AddMinutes(15);
        Timeslot actualTimeslot = Timeslot.Create(actualStartTime, actualEndTime);
        string notes = $"Mechanic notes {maintenanceJobBuilder.JobId}";

        // act
        sut.Finish(actualTimeslot, notes);

        // assert
        Assert.Equal(maintenanceJobBuilder.JobId, sut.Id);
        Assert.Equal(maintenanceJobBuilder.StartTime, sut.PlannedTimeslot.StartTime);
        Assert.Equal(maintenanceJobBuilder.EndTime, sut.PlannedTimeslot.EndTime);
        Assert.Equal(maintenanceJobBuilder.CustomerBuilder.Id, sut.Customer.Id);
        Assert.Equal(maintenanceJobBuilder.CustomerBuilder.Name, sut.Customer.Name);
        Assert.Equal(maintenanceJobBuilder.CustomerBuilder.TelephoneNumber, sut.Customer.TelephoneNumber);
        Assert.Equal(maintenanceJobBuilder.VehicleBuilder.LicenseNumber, sut.Vehicle.Id);
        Assert.Equal(maintenanceJobBuilder.VehicleBuilder.Brand, sut.Vehicle.Brand);
        Assert.Equal(maintenanceJobBuilder.VehicleBuilder.Type, sut.Vehicle.Type);
        Assert.Equal(maintenanceJobBuilder.VehicleBuilder.OwnerId, sut.Vehicle.OwnerId);
        Assert.Equal(maintenanceJobBuilder.Description, sut.Description);
        Assert.Equal(actualTimeslot, sut.ActualTimeslot);
        Assert.Equal(notes, sut.Notes);
        Assert.Equal("Completed", sut.Status);
    }
}