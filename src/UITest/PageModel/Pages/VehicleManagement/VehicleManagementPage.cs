namespace Pitstop.UITest.PageModel.Pages.VehicleManagement;

/// <summary>
/// Represents the VehicleManagement page.
/// </summary>
public class VehicleManagementPage : PitstopPage
{
    public VehicleManagementPage(PitstopApp pitstop) : base("Vehicle Management - overview", pitstop)
    {
    }

    public async Task<RegisterVehiclePage> RegisterVehicleAsync()
    {
        await Page.ClickAsync("#RegisterVehicleButton");
        return new RegisterVehiclePage(Pitstop);
    }

    public async Task<VehicleDetailsPage> SelectVehicleAsync(string licenseNumber)
    {
        await Page.ClickAsync($"//td[contains(text(),'{licenseNumber}')]");
        return new VehicleDetailsPage(Pitstop);
    }
}