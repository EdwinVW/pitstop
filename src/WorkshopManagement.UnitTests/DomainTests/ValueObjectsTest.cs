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
            Should.Throw<InvalidValueException>(() => LicenseNumber.Create(licenseNumber));

        // assert
        thrownException.Message.ShouldBe($"The specified license-number '{licenseNumber}' is not in the correct format.");
    }

    [TestMethod]
    public void Creating_A_TimeSlot_With_A_StartTime_After_EndTime_Should_Throw_Exception()
    {
        // arrange
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddHours(-2);

        // act
        var thrownException =
            Should.Throw<InvalidValueException>(() => Timeslot.Create(startTime, endTime));

        // assert
        thrownException.Message.ShouldBe("The specified start-time may not be after the specified end-time.");
    }
}