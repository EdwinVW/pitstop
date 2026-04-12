namespace Pitstop.WorkshopManagement.UnitTests.DomainTests;

[TestClass]
public class TimeslotTests
{
    [TestMethod]
    public void OverlapsWith_PartialOverlapLeft_ReturnsTrue()
    {
        var slot1 = Timeslot.Create(Today(8), Today(12));
        var slot2 = Timeslot.Create(Today(10), Today(14));

        Assert.IsTrue(slot1.OverlapsWith(slot2));
    }

    [TestMethod]
    public void OverlapsWith_PartialOverlapRight_ReturnsTrue()
    {
        var slot1 = Timeslot.Create(Today(10), Today(14));
        var slot2 = Timeslot.Create(Today(8), Today(12));

        Assert.IsTrue(slot1.OverlapsWith(slot2));
    }

    [TestMethod]
    public void OverlapsWith_SecondSlotWithinFirst_ReturnsTrue()
    {
        // job 1: 09:00-12:00, job 2: 10:00-11:00
        var slot1 = Timeslot.Create(Today(9), Today(12));
        var slot2 = Timeslot.Create(Today(10), Today(11));

        Assert.IsTrue(slot1.OverlapsWith(slot2));
    }

    [TestMethod]
    public void OverlapsWith_FirstSlotWithinSecond_ReturnsTrue()
    {
        var slot1 = Timeslot.Create(Today(10), Today(11));
        var slot2 = Timeslot.Create(Today(9), Today(12));

        Assert.IsTrue(slot1.OverlapsWith(slot2));
    }

    [TestMethod]
    public void OverlapsWith_IdenticalSlots_ReturnsTrue()
    {
        var slot1 = Timeslot.Create(Today(9), Today(12));
        var slot2 = Timeslot.Create(Today(9), Today(12));

        Assert.IsTrue(slot1.OverlapsWith(slot2));
    }

    [TestMethod]
    public void OverlapsWith_NoOverlap_ReturnsFalse()
    {
        var slot1 = Timeslot.Create(Today(8), Today(10));
        var slot2 = Timeslot.Create(Today(12), Today(14));

        Assert.IsFalse(slot1.OverlapsWith(slot2));
    }

    [TestMethod]
    public void OverlapsWith_TouchingEndpoints_ReturnsFalse()
    {
        // Adjacent slots (one ends when the other starts) should not overlap
        var slot1 = Timeslot.Create(Today(8), Today(10));
        var slot2 = Timeslot.Create(Today(10), Today(12));

        Assert.IsFalse(slot1.OverlapsWith(slot2));
    }

    [TestMethod]
    public void OverlapsWith_DateTimeOverload_ReturnsTrue()
    {
        var slot = Timeslot.Create(Today(9), Today(12));

        Assert.IsTrue(slot.OverlapsWith(Today(10), Today(11)));
    }

    private static DateTime Today(int hour) => DateTime.Today.AddHours(hour);
}
