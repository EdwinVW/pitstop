namespace Pitstop.UITest.PageModel.Pages.CustomerManagement;

/// <summary>
/// Represents the CustomerManagement page.
/// </summary>
public class CustomerManagementPage : PitstopPage
{
    public CustomerManagementPage(PitstopApp pitstop) : base("Customer Management - overview", pitstop)
    {
    }

    public async Task<RegisterCustomerPage> RegisterCustomerAsync()
    {
        await Page.ClickAsync("#RegisterCustomerButton");
        return new RegisterCustomerPage(Pitstop);
    }

    public async Task<CustomerDetailsPage> SelectCustomerAsync(string customerName)
    {
        await Page.ClickAsync($"//td[contains(text(),'{customerName}')]");
        return new CustomerDetailsPage(Pitstop); 
    }
}