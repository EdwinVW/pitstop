namespace Pitstop.UITest.PageModel;

public class MainMenu
{
    private readonly PitstopApp _pitStop;

    public MainMenu(PitstopApp pitstop)
    {
        _pitStop = pitstop;
    }

    public async Task<HomePage> HomeAsync()
    {
        await _pitStop.Page.ClickAsync("#MainMenu\\.Home");
        return new HomePage(_pitStop);
    }

    public async Task<CustomerManagementPage> CustomerManagementAsync()
    {
        await _pitStop.Page.ClickAsync("#MainMenu\\.CustomerManagement");
        return new CustomerManagementPage(_pitStop);
    }

    public async Task<VehicleManagementPage> VehicleManagementAsync()
    {
        await _pitStop.Page.ClickAsync("#MainMenu\\.VehicleManagement");
        return new VehicleManagementPage(_pitStop);
    }

    public async Task<WorkshopManagementPage> WorkshopManagementAsync()
    {
        await _pitStop.Page.ClickAsync("#MainMenu\\.WorkshopManagement");
        return new WorkshopManagementPage(_pitStop);
    }

    public async Task<AboutPage> AboutAsync()
    {
        await _pitStop.Page.ClickAsync("#MainMenu\\.About");
        return new AboutPage(_pitStop);
    }
}