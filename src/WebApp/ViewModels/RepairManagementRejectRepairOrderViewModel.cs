namespace Pitstop.WebApp.ViewModels;

public class RepairManagementRejectRepairOrderViewModel
{
    public string RepairOrderId { get; set; }

    [Display(Name = "Reason for Rejection:")]
    [Required(ErrorMessage = "Please provide a reason for rejection.")]
    public string RejectReason { get; set; }
}