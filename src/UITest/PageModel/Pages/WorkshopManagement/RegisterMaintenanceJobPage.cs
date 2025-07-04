namespace Pitstop.UITest.PageModel.Pages.WorkshopManagement;

/// <summary>
/// Represents the Register MaintenanceJob page.
/// </summary>
public class RegisterMaintenanceJobPage : PitstopPage
{
    public RegisterMaintenanceJobPage(PitstopApp pitstop) : base("Workshop Management - schedule maintenance", pitstop)
    {
    }

    public async Task<RegisterMaintenanceJobPage> FillJobDetailsAsync(string startTime, string endTime, string description, string licenseNumber)
    {
        await Page.FillAsync("[name=\"StartTime\"]", startTime);
        await Page.FillAsync("[name=\"EndTime\"]", endTime);
        await Page.FillAsync("[name=\"Description\"]", description);

        await Page.SelectOptionAsync("#SelectedVehicleLicenseNumber", licenseNumber);

        return this;
    }

    public async Task<WorkshopManagementPage> SubmitAsync()
    {
        await Page.ClickAsync("#SubmitButton");
        return new WorkshopManagementPage(Pitstop);
    }

    public async Task<WorkshopManagementPage> CancelAsync()
    {
        await Page.ClickAsync("#CancelButton");
        return new WorkshopManagementPage(Pitstop);
    }
}