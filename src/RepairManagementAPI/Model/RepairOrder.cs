using Newtonsoft.Json;

namespace Pitstop.RepairManagementAPI.Model;

public class RepairOrder
{
    public Guid Id { get; set; }

    // Customer data
    public string CutomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }

    //Vehicle data
    public string LicenseNumber { get; set; }
    public string Type { get; set; }
    public string Brand { get; set; }
    public decimal TotalCost { get; set; }
    public decimal LaborCost { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Status { get; set; }
    public string RejectReason { get; set; }

    
    [JsonIgnore]
    public ICollection<RepairOrderVehicleParts> RepairOrderVehicleParts { get; set; } = new List<RepairOrderVehicleParts>();
}