namespace Pitstop.UITest.PageModel.Pages.CustomerManagement;

/// <summary>
/// Represents the CustomerDetails page.
/// </summary>
public class CustomerDetailsPage : PitstopPage
{
    public CustomerDetailsPage(PitstopApp pitstop) : base("Customer Management - details", pitstop)
    {
    }

    public async Task<CustomerManagementPage> BackAsync()
    {
        await Page.ClickAsync("#BackButton");
        return new CustomerManagementPage(Pitstop);
    }
}