namespace Pitstop.UITest.PageModel.Pages.CustomerManagement;

/// <summary>
/// Represents the RegisterCustomer page.
/// </summary>
public class RegisterCustomerPage : PitstopPage
{
    public RegisterCustomerPage(PitstopApp pitstop) : base("Customer Management - register customer", pitstop)
    {
    }

    public async Task<RegisterCustomerPage> FillCustomerDetailsAsync(string name, string address,
        string city, string postalCode, string telephoneNumber, string emailAddress)
    {
        await Page.FillAsync("[name=\"Customer.Name\"]", name);
        await Page.FillAsync("[name=\"Customer.Address\"]", address);
        await Page.FillAsync("[name=\"Customer.PostalCode\"]", postalCode);
        await Page.FillAsync("[name=\"Customer.City\"]", city);
        await Page.FillAsync("[name=\"Customer.TelephoneNumber\"]", telephoneNumber);
        await Page.FillAsync("[name=\"Customer.EmailAddress\"]", emailAddress);
        return this;
    }

    public async Task<CustomerManagementPage> SubmitAsync()
    {
        await Page.ClickAsync("#SubmitButton");
        return new CustomerManagementPage(Pitstop);
    }

    public async Task<CustomerManagementPage> CancelAsync()
    {
        await Page.ClickAsync("#CancelButton");
        return new CustomerManagementPage(Pitstop);
    }
}