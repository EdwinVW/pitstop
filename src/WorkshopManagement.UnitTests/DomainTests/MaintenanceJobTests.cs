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
        Assert.AreEqual(maintenanceJobBuilder.JobId, sut.Id);
        Assert.AreEqual(maintenanceJobBuilder.StartTime, sut.PlannedTimeslot.StartTime);
        Assert.AreEqual(maintenanceJobBuilder.EndTime, sut.PlannedTimeslot.EndTime);
        Assert.AreEqual(maintenanceJobBuilder.CustomerBuilder.Id, sut.Customer.Id);
        Assert.AreEqual(maintenanceJobBuilder.CustomerBuilder.Name, sut.Customer.Name);
        Assert.AreEqual(maintenanceJobBuilder.CustomerBuilder.TelephoneNumber, sut.Customer.TelephoneNumber);
        Assert.AreEqual(maintenanceJobBuilder.VehicleBuilder.LicenseNumber, sut.Vehicle.Id);
        Assert.AreEqual(maintenanceJobBuilder.VehicleBuilder.Brand, sut.Vehicle.Brand);
        Assert.AreEqual(maintenanceJobBuilder.VehicleBuilder.Type, sut.Vehicle.Type);
        Assert.AreEqual(maintenanceJobBuilder.VehicleBuilder.OwnerId, sut.Vehicle.OwnerId);
        Assert.AreEqual(maintenanceJobBuilder.Description, sut.Description);
        Assert.IsNull(sut.ActualTimeslot);
        Assert.IsNull(sut.Notes);
        Assert.AreEqual("Planned", sut.Status);
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
        Assert.AreEqual(maintenanceJobBuilder.JobId, sut.Id);
        Assert.AreEqual(maintenanceJobBuilder.StartTime, sut.PlannedTimeslot.StartTime);
        Assert.AreEqual(maintenanceJobBuilder.EndTime, sut.PlannedTimeslot.EndTime);
        Assert.AreEqual(maintenanceJobBuilder.CustomerBuilder.Id, sut.Customer.Id);
        Assert.AreEqual(maintenanceJobBuilder.CustomerBuilder.Name, sut.Customer.Name);
        Assert.AreEqual(maintenanceJobBuilder.CustomerBuilder.TelephoneNumber, sut.Customer.TelephoneNumber);
        Assert.AreEqual(maintenanceJobBuilder.VehicleBuilder.LicenseNumber, sut.Vehicle.Id);
        Assert.AreEqual(maintenanceJobBuilder.VehicleBuilder.Brand, sut.Vehicle.Brand);
        Assert.AreEqual(maintenanceJobBuilder.VehicleBuilder.Type, sut.Vehicle.Type);
        Assert.AreEqual(maintenanceJobBuilder.VehicleBuilder.OwnerId, sut.Vehicle.OwnerId);
        Assert.AreEqual(maintenanceJobBuilder.Description, sut.Description);
        Assert.AreEqual(actualTimeslot, sut.ActualTimeslot);
        Assert.AreEqual(notes, sut.Notes);
        Assert.AreEqual("Completed", sut.Status);
    }
}