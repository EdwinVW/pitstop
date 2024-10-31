namespace Pitstop.WebApp.Models;

public class Rejection
{
    public Guid RepairOrderId;
    
    [Display(Name = "Rejected at")]
    public DateTime RejectedAt;
    
    [Display(Name = "Rejection reason")]
    public string RejectReason;
}