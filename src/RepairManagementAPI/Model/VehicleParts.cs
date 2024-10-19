namespace Pitstop.RepairManagementAPI.Model;

public class VehicleParts
{
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }

    public VehicleParts(Guid id, string name, decimal cost)
    {
        Id = id;
        Name = name;
        Cost = cost;
    }
}