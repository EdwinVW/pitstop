namespace Pitstop.RepairManagementAPI.Model;

public class VehicleParts
{
    
    public Guid Id { get; set; }
    public string PartName { get; set; }
    public decimal PartCost { get; set; }

    public VehicleParts(Guid id, string partName, decimal partCost)
    {
        Id = id;
        PartName = partName;
        PartCost = partCost;
    }
}