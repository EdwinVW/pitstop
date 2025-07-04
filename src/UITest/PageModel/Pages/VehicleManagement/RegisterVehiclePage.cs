namespace Pitstop.UITest.PageModel.Pages.VehicleManagement;

/// <summary>
/// Represents the RegisterVehicle page.
/// </summary>
public class RegisterVehiclePage : PitstopPage
{
    public RegisterVehiclePage(PitstopApp pitstop) : base("Vehicle Management - register vehicle", pitstop)
    {
    }

    public async Task<RegisterVehiclePage> FillVehicleDetailsAsync(string licenseNumber, string brand, string type, string owner)
    {
        await Page.FillAsync("[name=\"Vehicle.LicenseNumber\"]", licenseNumber);
        await Page.FillAsync("[name=\"Vehicle.Brand\"]", brand);
        await Page.FillAsync("[name=\"Vehicle.Type\"]", type);
        await Page.SelectOptionAsync("#SelectedCustomerId", new SelectOptionValue { Label = owner });
        return this;
    }

    public async Task<VehicleManagementPage> SubmitAsync()
    {
        await Page.ClickAsync("#SubmitButton");
        return new VehicleManagementPage(Pitstop);
    }

    public async Task<VehicleManagementPage> CancelAsync()
    {
        await Page.ClickAsync("#CancelButton");
        return new VehicleManagementPage(Pitstop);
    }
}