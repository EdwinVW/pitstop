namespace Pitstop.WorkshopManagement.UnitTests.TestdataBuilders;

public class CustomerBuilder
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string TelephoneNumber { get; private set; }

    public CustomerBuilder()
    {
        SetDefaults();
    }

    public CustomerBuilder WithId(string id)
    {
        Id = id;
        return this;
    }

    public CustomerBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public CustomerBuilder WithTelephoneNumber(string telephoneNumber)
    {
        TelephoneNumber = telephoneNumber;
        return this;
    }

    public Customer Build()
    {
        return new Customer(Id, Name, TelephoneNumber);
    }

    private void SetDefaults()
    {
        Id = Guid.NewGuid().ToString();
        Name = "John Doe";
        TelephoneNumber = "+31612345678";
    }
}