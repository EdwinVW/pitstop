namespace Pitstop.RepairManagementAPI.Model;

public class RepairOrders
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; }
    public string LicenseNumber { get; set; }
    public decimal TotalCost { get; set; }
    public List<string> VehiclePartId  { get; set; }
    public string LaborCost { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public RepairOrdersStatus Status { get; set; }
    public string RejectReason { get; set; }
}