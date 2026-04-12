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

    /// <summary>
    /// Submit the form expecting a business rule violation. The page re-renders with an error message.
    /// </summary>
    public async Task<RegisterMaintenanceJobPage> SubmitExpectingErrorAsync()
    {
        await Page.ClickAsync("#SubmitButton");
        return this;
    }

    /// <summary>
    /// Returns the business rule violation error message, or null if no error is shown.
    /// The error is displayed in a div.alert-danger on the New view.
    /// </summary>
    public async Task<string> GetErrorMessageAsync()
    {
        var errorElement = await Page.QuerySelectorAsync(".alert-danger label");
        if (errorElement == null) return null;
        return await errorElement.TextContentAsync();
    }

    public async Task<WorkshopManagementPage> CancelAsync()
    {
        await Page.ClickAsync("#CancelButton");
        return new WorkshopManagementPage(Pitstop);
    }
}