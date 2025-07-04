namespace WorkshopManagement.UnitTests.DomainTests;

internal class TestValue : ValueObject
{
    public string Value { get; private set; }

    public TestValue(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

[TestClass]
public class CoreTests
{
    [TestMethod]
    public void ValueObject_Equality_Should_Detect_Equal_Values()
    {
        // arrange
        TestValue value1 = new TestValue("TestValue");
        TestValue value2 = new TestValue("TestValue");

        // act
        bool implicitEqual = value1 == value2;
        bool explicitEqual = value1.Equals(value2);

        // assert
        Assert.IsTrue(implicitEqual);
        Assert.IsTrue(explicitEqual);
    }

    [TestMethod]
    public void ValueObject_Equality_Should_Detect_NotEqual_Values()
    {
        // arrange
        TestValue value1 = new TestValue("TestValue 1");
        TestValue value2 = new TestValue("TestValue 2");

        // act
        bool implicitEqual = value1 != value2;
        bool explicitEqual = !value1.Equals(value2);

        // assert
        Assert.IsTrue(implicitEqual);
        Assert.IsTrue(explicitEqual);
    }
}