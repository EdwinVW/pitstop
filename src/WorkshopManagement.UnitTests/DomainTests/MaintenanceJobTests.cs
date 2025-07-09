namespace WorkshopManagement.UnitTests.DomainTests;

[TestClass]
public class MaintenanceJobTests
{
    [TestMethod]
    public void Plan_Should_Create_A_New_Job()
    {
        // arrange
        MaintenanceJobBuilder maintenanceJobBuilder = new MaintenanceJobBuilder();
        MaintenanceJob sut = maintenanceJobBuilder
            .Build();

        // act
        // sut.Plan() is called by the Testdata Builder

        // assert
        sut.Id.ShouldBe(maintenanceJobBuilder.JobId);
        sut.PlannedTimeslot.StartTime.ShouldBe(maintenanceJobBuilder.StartTime);
        sut.PlannedTimeslot.EndTime.ShouldBe(maintenanceJobBuilder.EndTime);
        sut.Customer.Id.ShouldBe(maintenanceJobBuilder.CustomerBuilder.Id);
        sut.Customer.Name.ShouldBe(maintenanceJobBuilder.CustomerBuilder.Name);
        sut.Customer.TelephoneNumber.ShouldBe(maintenanceJobBuilder.CustomerBuilder.TelephoneNumber);
        sut.Vehicle.Id.ShouldBe(maintenanceJobBuilder.VehicleBuilder.LicenseNumber);
        sut.Vehicle.Brand.ShouldBe(maintenanceJobBuilder.VehicleBuilder.Brand);
        sut.Vehicle.Type.ShouldBe(maintenanceJobBuilder.VehicleBuilder.Type);
        sut.Vehicle.OwnerId.ShouldBe(maintenanceJobBuilder.VehicleBuilder.OwnerId);
        sut.Description.ShouldBe(maintenanceJobBuilder.Description);
        sut.ActualTimeslot.ShouldBeNull();
        sut.Notes.ShouldBeNull();
        sut.Status.ShouldBe("Planned");
    }

    [TestMethod]
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
        sut.Id.ShouldBe(maintenanceJobBuilder.JobId);
        sut.PlannedTimeslot.StartTime.ShouldBe(maintenanceJobBuilder.StartTime);
        sut.PlannedTimeslot.EndTime.ShouldBe(maintenanceJobBuilder.EndTime);
        sut.Customer.Id.ShouldBe(maintenanceJobBuilder.CustomerBuilder.Id);
        sut.Customer.Name.ShouldBe(maintenanceJobBuilder.CustomerBuilder.Name);
        sut.Customer.TelephoneNumber.ShouldBe(maintenanceJobBuilder.CustomerBuilder.TelephoneNumber);
        sut.Vehicle.Id.ShouldBe(maintenanceJobBuilder.VehicleBuilder.LicenseNumber);
        sut.Vehicle.Brand.ShouldBe(maintenanceJobBuilder.VehicleBuilder.Brand);
        sut.Vehicle.Type.ShouldBe(maintenanceJobBuilder.VehicleBuilder.Type);
        sut.Vehicle.OwnerId.ShouldBe(maintenanceJobBuilder.VehicleBuilder.OwnerId);
        sut.Description.ShouldBe(maintenanceJobBuilder.Description);
        sut.ActualTimeslot.ShouldBe(actualTimeslot);
        sut.Notes.ShouldBe(notes);
        sut.Status.ShouldBe("Completed");
    }
}