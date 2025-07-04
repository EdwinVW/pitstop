namespace Pitstop.UITest.PageModel.Pages.WorkshopManagement;

/// <summary>
/// Represents the Finish MaintenanceJob page.
/// </summary>
public class FinishMaintenanceJobPage : PitstopPage
{
    public FinishMaintenanceJobPage(PitstopApp pitstop) : base("Workshop Management - finish maintenance job", pitstop)
    {
    }

    public async Task<FinishMaintenanceJobPage> FillJobDetailsAsync(string actualStartTime, string actualEndTime, string notes)
    {
        await Page.FillAsync("[name=\"ActualStartTime\"]", actualStartTime);
        await Page.FillAsync("[name=\"ActualEndTime\"]", actualEndTime);
        await Page.FillAsync("[name=\"Notes\"]", notes);

        return this;
    }

    public async Task<MaintenanceJobDetailsPage> CompleteAsync()
    {
        await Page.ClickAsync("#CompleteButton");
        return new MaintenanceJobDetailsPage(Pitstop);
    }

    public async Task<MaintenanceJobDetailsPage> CancelAsync()
    {
        await Page.ClickAsync("#CancelButton");
        return new MaintenanceJobDetailsPage(Pitstop);
    }
}