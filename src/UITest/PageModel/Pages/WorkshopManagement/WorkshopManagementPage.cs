namespace Pitstop.UITest.PageModel.Pages.WorkshopManagement;

/// <summary>
/// Represents the WorkshopManagement page.
/// </summary>
public class WorkshopManagementPage : PitstopPage
{
    public WorkshopManagementPage(PitstopApp pitstop) : base("Workshop Management - overview", pitstop)
    {
    }

    public async Task<RegisterMaintenanceJobPage> RegisterMaintenanceJobAsync()
    {
        await Page.ClickAsync("#RegisterMaintenanceJobButton");
        return new RegisterMaintenanceJobPage(Pitstop);
    }

    public async Task<MaintenanceJobDetailsPage> SelectMaintenanceJobAsync(string jobDescription)
    {
        await Page.ClickAsync($"//td[contains(text(),'{jobDescription}')]");
        return new MaintenanceJobDetailsPage(Pitstop);
    }
}