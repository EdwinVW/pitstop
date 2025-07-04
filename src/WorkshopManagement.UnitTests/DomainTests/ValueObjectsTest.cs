namespace WorkshopManagement.UnitTests.DomainTests;

[TestClass]
public class ValueObjectsTest
{
    [TestMethod]
    public void Creating_A_LicenseNumber_With_An_Invalid_Format_Should_Throw_Exception()
    {
        // arrange
        string licenseNumber = "123456";

        // act
        var thrownException =
            Assert.ThrowsException<InvalidValueException>(() => LicenseNumber.Create(licenseNumber));

        // assert
        Assert.AreEqual($"The specified license-number '{licenseNumber}' is not in the correct format.",
            thrownException.Message);
    }

    [TestMethod]
    public void Creating_A_TimeSlot_With_A_StartTime_After_EndTime_Should_Throw_Exception()
    {
        // arrange
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddHours(-2);

        // act
        var thrownException =
            Assert.ThrowsException<InvalidValueException>(() => Timeslot.Create(startTime, endTime));

        // assert
        Assert.AreEqual("The specified start-time may not be after the specified end-time.",
            thrownException.Message);
    }
}