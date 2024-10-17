namespace Pitstop.WebApp.Models;

public class Repair

{
    public string CustomerId { get; set; }
    
    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; }

    [Display(Name = "Customer Email Address")]
    public string CustomerEmailAddress { get; set; }

    [Display(Name = "License Number")]
    public string LicenseNumber { get; set; }

    [Display(Name = "Approval Status")]
    public string RepairStatus         {
            get
            {
                return ApprovalRequest? "Approval Request Sent" : "Planned";
            }
        }

    [Display(Name = "Approval Request")]
    public bool ApprovalRequest { get; set; }
}