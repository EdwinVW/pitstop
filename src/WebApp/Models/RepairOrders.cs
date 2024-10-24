namespace Pitstop.WebApp.Models
{
    public class RepairOrder
    {
        public Guid Id { get; set; }

        [Display(Name = "Customer Info")] 
        public CustomerInfo CustomerInfo { get; set; }

        [Display(Name = "Vehicle Info")] 
        public VehicleInfo VehicleInfo { get; set; }


        [Display(Name = "Total Cost")]
        [DataType(DataType.Currency)]
        public decimal TotalCost { get; set; }


        [Display(Name = "Select Vehicle Parts for Repair")]
        public List<Guid> ToRepairVehiclePartIds { get; set; }

        [Display(Name = "Vehicle Parts for Repair")]
        public List<VehicleParts> ToRepairVehicleParts { get; set; }

        [Display(Name = "Labor Cost")]
        [DataType(DataType.Currency)]
        public decimal LaborCost { get; set; }

        public bool IsApproved { get; set; }

        [DataType(DataType.Date)] public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Status")] public string Status { get; set; }

        [Display(Name = "Reject Reason")] public string RejectReason { get; set; }
    }

    public class CustomerInfo
    {
        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Customer Email")] public string CustomerEmail { get; set; }

        [Display(Name = "Customer Phone")] public string CustomerPhone { get; set; }
    }

    public class VehicleInfo
    {
        [Required]
        [Display(Name = "License Number")]
        public string LicenseNumber { get; set; }

        [Display(Name = "Brand")] public string Brand { get; set; }

        [Display(Name = "Type")] public string Type { get; set; }
    }

    public class VehicleParts
    {
        public string Id { get; set; }

        [Display(Name = "Part Name")] 
        public string PartName { get; set; }

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
}