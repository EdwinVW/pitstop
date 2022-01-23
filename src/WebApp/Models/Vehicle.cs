namespace Pitstop.WebApp.Models;

public class Vehicle
{
    [Required]
    [Display(Name = "License number")]
    [RegularExpression(@"^((\d{1,3}|[a-zA-Z]{1,3})-){2}(\d{1,3}|[a-zA-Z]{1,3})$", ErrorMessage = "LicenseNumber is not in a valid format.")]
    public string LicenseNumber { get; set; }

    [Required]
    [Display(Name = "Brand")]
    public string Brand { get; set; }

    [Required]
    [Display(Name = "Type")]
    public string Type { get; set; }

    [Display(Name = "Owner")]
    public string OwnerId { get; set; }
}