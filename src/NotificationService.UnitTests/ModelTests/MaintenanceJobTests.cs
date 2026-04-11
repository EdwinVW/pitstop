namespace NotificationService.UnitTests.ModelTests;

[TestClass]
public class MaintenanceJobTests
{
    [TestMethod]
    public void CreateFrom_Should_Map_All_Fields_From_MaintenanceJobPlanned_Event()
    {
        // arrange
        MaintenanceJobPlannedEventBuilder builder = new MaintenanceJobPlannedEventBuilder();
        MaintenanceJobPlanned ev = builder.Build();

        // act
        MaintenanceJob sut = MaintenanceJob.CreateFrom(ev);

        // assert
        Assert.AreEqual(builder.JobId.ToString(), sut.JobId);
        Assert.AreEqual(builder.CustomerId, sut.CustomerId);
        Assert.AreEqual(builder.LicenseNumber, sut.LicenseNumber);
        Assert.AreEqual(builder.StartTime, sut.StartTime);
        Assert.AreEqual(builder.Description, sut.Description);
    }
}
