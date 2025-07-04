namespace Pitstop.UITest.PageModel.Pages.VehicleManagement;

/// <summary>
/// Represents the VehicleDetails page.
/// </summary>
public class VehicleDetailsPage : PitstopPage
{        
    public VehicleDetailsPage(PitstopApp pitstop) : base("Vehicle Management - details", pitstop)
    {
    }

    public async Task<VehicleManagementPage> BackAsync()
    {
        await Page.ClickAsync("#BackButton");
        return new VehicleManagementPage(Pitstop);
    }
}