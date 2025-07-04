namespace Pitstop.UITest.PageModel.Pages.WorkshopManagement;

/// <summary>
/// Represents the MaintenanceJob Details page.
/// </summary>
public class MaintenanceJobDetailsPage : PitstopPage
{
    public MaintenanceJobDetailsPage(PitstopApp pitstop) : base("Workshop Management - details", pitstop)
    {
    }

    public async Task<FinishMaintenanceJobPage> CompleteAsync()
    {
        await Page.ClickAsync("#CompleteButton");
        return new FinishMaintenanceJobPage(Pitstop);
    }

    public async Task<WorkshopManagementPage> BackAsync()
    {
        await Page.ClickAsync("#BackButton");
        return new WorkshopManagementPage(Pitstop);
    }

    public async Task<string> GetJobStatusAsync()
    {
        var statusElement = await Page.QuerySelectorAsync("#JobStatus");
        return await statusElement.TextContentAsync();
    }
}