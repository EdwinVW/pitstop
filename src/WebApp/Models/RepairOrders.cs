namespace Pitstop.WebApp.Models;

public class RepairOrders
{
    public string Id { get; set; }

    [Required]
    [Display(Name = "Customer ID")]
    public string CustomerId { get; set; }

    [Required]
    [Display(Name = "License Number")]
    public string LicenseNumber { get; set; }

    [Required]
    [Display(Name = "Total Cost")]
    [DataType(DataType.Currency)]
    public decimal TotalCost { get; set; }

    [Required]
    [Display(Name = "Vehicle Parts")]
    public List<string> VehiclePartIds { get; set; } = new List<string>();

    [Required]
    [Display(Name = "Labor Cost")]
    [DataType(DataType.Currency)]
    public string LaborCost { get; set; }

    [Display(Name = "Is Approved")] public bool IsApproved { get; set; }

    [Required]
    [Display(Name = "Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Updated At")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; }

    [Required] [Display(Name = "Status")] public RepairOrdersStatus Status { get; set; }

    [Display(Name = "Reject Reason")] public string RejectReason { get; set; }
}

public class VehicleParts
{
    public string Id { get; set; }

    [Required]
    [Display(Name = "Part Name")]
    public string PartName { get; set; }

    [Required]
    [Display(Name = "Part Cost")]
    [DataType(DataType.Currency)]
    public decimal PartCost { get; set; }
}

public enum RepairOrdersStatus
{
    Approved,
    Rejected,
    Sent,
    NotCreatedYet
}