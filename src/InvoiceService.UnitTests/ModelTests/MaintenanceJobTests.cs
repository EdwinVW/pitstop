namespace InvoiceService.UnitTests.ModelTests;

[TestClass]
public class MaintenanceJobTests
{
    [TestMethod]
    public void CreateFrom_Should_Map_All_Fields_From_MaintenanceJobPlanned_Event()
    {
        // arrange
        MaintenanceJobPlannedEventBuilder builder = new MaintenanceJobPlannedEventBuilder();
        Pitstop.InvoiceService.Events.MaintenanceJobPlanned ev = builder.Build();

        // act
        MaintenanceJob sut = MaintenanceJob.CreateFrom(ev);

        // assert
        Assert.AreEqual(builder.JobId, sut.JobId);
        Assert.AreEqual(builder.CustomerId, sut.CustomerId);
        Assert.AreEqual(builder.LicenseNumber, sut.LicenseNumber);
        Assert.AreEqual(builder.Description, sut.Description);
    }

    [TestMethod]
    public void CreateFrom_Should_Set_Finished_And_InvoiceSent_To_False()
    {
        // arrange
        Pitstop.InvoiceService.Events.MaintenanceJobPlanned ev =
            new MaintenanceJobPlannedEventBuilder().Build();

        // act
        MaintenanceJob sut = MaintenanceJob.CreateFrom(ev);

        // assert
        Assert.IsNull(sut.StartTime);
        Assert.IsNull(sut.EndTime);
        Assert.IsFalse(sut.Finished);
        Assert.IsFalse(sut.InvoiceSent);
    }
}
