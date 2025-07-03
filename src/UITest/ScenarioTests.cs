namespace Pitstop.UITest;

public class ScenarioTests
{
    private readonly ITestOutputHelper _output;

    public ScenarioTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task End_To_End()
    {
        // arrange
        string testrunId = Guid.NewGuid().ToString("N");
        PitstopApp pitstop = await PitstopApp.CreateAsync(testrunId, TestConstants.PitstopStartUrl);
        var homePage = await pitstop.StartAsync();
        string licenseNumber = TestDataGenerators.GenerateRandomLicenseNumber();

        // act
        var customerManagementPage = await pitstop.Menu.CustomerManagementAsync();
        var registerCustomerPage = await customerManagementPage.RegisterCustomerAsync();
        var cancelledPage = await registerCustomerPage.CancelAsync();
        var registerCustomerPage2 = await cancelledPage.RegisterCustomerAsync();
        var filledPage = await registerCustomerPage2.FillCustomerDetailsAsync(
            $"TestCustomer {testrunId}", "Verzonnenstraat 21",
            "Uitdeduimerveen", "1234 AZ", "+31612345678", "tc@test.com");
        var submittedPage = await filledPage.SubmitAsync();
        var customerDetailsPage = await submittedPage.SelectCustomerAsync($"TestCustomer {testrunId}");
        await customerDetailsPage.BackAsync();

        var vehicleManagementPage = await pitstop.Menu.VehicleManagementAsync();
        var registerVehiclePage = await vehicleManagementPage.RegisterVehicleAsync();
        var cancelledVehiclePage = await registerVehiclePage.CancelAsync();
        var registerVehiclePage2 = await cancelledVehiclePage.RegisterVehicleAsync();
        var filledVehiclePage = await registerVehiclePage2.FillVehicleDetailsAsync(licenseNumber, "Testla", "Model T", $"TestCustomer {testrunId}");
        var submittedVehiclePage = await filledVehiclePage.SubmitAsync();
        var vehicleDetailsPage = await submittedVehiclePage.SelectVehicleAsync(licenseNumber);
        await vehicleDetailsPage.BackAsync();

        var workshopManagementPage = await pitstop.Menu.WorkshopManagementAsync();
        var registerMaintenanceJobPage = await workshopManagementPage.RegisterMaintenanceJobAsync();
        var cancelledJobPage = await registerMaintenanceJobPage.CancelAsync();
        var registerMaintenanceJobPage2 = await cancelledJobPage.RegisterMaintenanceJobAsync();
        var filledJobPage = await registerMaintenanceJobPage2.FillJobDetailsAsync("08:00", "12:00", $"Job {testrunId}", licenseNumber);
        var submittedJobPage = await filledJobPage.SubmitAsync();
        var jobDetailsPage = await submittedJobPage.SelectMaintenanceJobAsync($"Job {testrunId}");
        await jobDetailsPage.BackAsync();

        var workshopManagementPage2 = await pitstop.Menu.WorkshopManagementAsync();
        var jobDetailsPage2 = await workshopManagementPage2.SelectMaintenanceJobAsync($"Job {testrunId}");
        var beforeJobStatus = await jobDetailsPage2.GetJobStatusAsync();
        var finishJobPage = await jobDetailsPage2.CompleteAsync();
        var filledFinishJobPage = await finishJobPage.FillJobDetailsAsync("08:00", "11:00", $"Mechanic notes {testrunId}");
        var completedJobPage = await filledFinishJobPage.CompleteAsync();
        await completedJobPage.BackAsync();

        var workshopManagementPage3 = await pitstop.Menu.WorkshopManagementAsync();
        var finalJobDetailsPage = await workshopManagementPage3.SelectMaintenanceJobAsync($"Job {testrunId}");
        var afterJobStatus = await finalJobDetailsPage.GetJobStatusAsync();
        await finalJobDetailsPage.BackAsync();

        // assert
        Assert.Equal("Planned", beforeJobStatus);
        Assert.Equal("Completed", afterJobStatus);

        // cleanup
        await pitstop.StopAsync();
    }
}