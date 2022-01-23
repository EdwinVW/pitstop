namespace Pitstop.WebApp.Models;

public class Customer
{
    public string CustomerId { get; set; }

    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Address")]
    public string Address { get; set; }

    [Required]
    [Display(Name = "Postal code")]
    public string PostalCode { get; set; }

    [Required]
    [Display(Name = "City")]
    public string City { get; set; }

    [Required]
    [Display(Name = "Phonenumber")]
    public string TelephoneNumber { get; set; }

    [Required]
    [Display(Name = "Email address")]
    [EmailAddress()]
    public string EmailAddress { get; set; }

    [Display(Name = "Loyalty level")]
    public int? LoyaltyLevel { get; set; }
}