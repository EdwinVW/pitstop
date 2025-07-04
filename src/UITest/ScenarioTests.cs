namespace Pitstop.UITest;

[TestClass]
public class ScenarioTests
{
    string _testrunId;
    PitstopApp _pitstopApp;
    string _licenseNumber;

    [TestInitialize]
    public async Task Initialize()
    {
        _testrunId = Guid.NewGuid().ToString("N");
        _licenseNumber = TestDataGenerators.GenerateRandomLicenseNumber();
        _pitstopApp = await PitstopApp.CreateAsync(_testrunId, TestConstants.PitstopStartUrl);
        await _pitstopApp.StartAsync();
    }

    [TestMethod]
    public async Task RegisterCustomer()
    {
        try
        {
            // Register Customer
            var customerManagementPage = await _pitstopApp.Menu.CustomerManagementAsync();
            var registerCustomerPage = await customerManagementPage.RegisterCustomerAsync();
            var cancelledPage = await registerCustomerPage.CancelAsync();
            var registerCustomerPage2 = await cancelledPage.RegisterCustomerAsync();
            var filledPage = await registerCustomerPage2.FillCustomerDetailsAsync(
                $"TestCustomer {_testrunId}", "Verzonnenstraat 21",
                "Uitdeduimerveen", "1234 AZ", "+31612345678", "tc@test.com");
            var submittedPage = await filledPage.SubmitAsync();
            var customerDetailsPage = await submittedPage.SelectCustomerAsync($"TestCustomer {_testrunId}");
            await customerDetailsPage.BackAsync();
    
            // Register Vehicle
            var vehicleManagementPage = await _pitstopApp.Menu.VehicleManagementAsync();
            var registerVehiclePage = await vehicleManagementPage.RegisterVehicleAsync();
            var cancelledVehiclePage = await registerVehiclePage.CancelAsync();
            var registerVehiclePage2 = await cancelledVehiclePage.RegisterVehicleAsync();
            var filledVehiclePage = await registerVehiclePage2.FillVehicleDetailsAsync(_licenseNumber, "Testla", "Model T", $"TestCustomer {_testrunId}");
            var submittedVehiclePage = await filledVehiclePage.SubmitAsync();
            var vehicleDetailsPage = await submittedVehiclePage.SelectVehicleAsync(_licenseNumber);
            await vehicleDetailsPage.BackAsync();

            // Register MaintenanceJob
            var workshopManagementPage = await _pitstopApp.Menu.WorkshopManagementAsync();
            var registerMaintenanceJobPage = await workshopManagementPage.RegisterMaintenanceJobAsync();
            var cancelledJobPage = await registerMaintenanceJobPage.CancelAsync();
            var registerMaintenanceJobPage2 = await cancelledJobPage.RegisterMaintenanceJobAsync();
            var filledJobPage = await registerMaintenanceJobPage2.FillJobDetailsAsync("08:00", "12:00", $"Job {_testrunId}", _licenseNumber);
            var submittedJobPage = await filledJobPage.SubmitAsync();
            var jobDetailsPage = await submittedJobPage.SelectMaintenanceJobAsync($"Job {_testrunId}");
            await jobDetailsPage.BackAsync();

            // Complete MaintenanceJob
            var workshopManagementPage2 = await _pitstopApp.Menu.WorkshopManagementAsync();
            var jobDetailsPage2 = await workshopManagementPage2.SelectMaintenanceJobAsync($"Job {_testrunId}");
            var beforeJobStatus = await jobDetailsPage2.GetJobStatusAsync();
            var finishJobPage = await jobDetailsPage2.CompleteAsync();
            var filledFinishJobPage = await finishJobPage.FillJobDetailsAsync("08:00", "11:00", $"Mechanic notes {_testrunId}");
            var completedJobPage = await filledFinishJobPage.CompleteAsync();
            await completedJobPage.BackAsync();

            // Select MaintenanceJob
            var workshopManagementPage3 = await _pitstopApp.Menu.WorkshopManagementAsync();
            var finalJobDetailsPage = await workshopManagementPage3.SelectMaintenanceJobAsync($"Job {_testrunId}");
            var afterJobStatus = await finalJobDetailsPage.GetJobStatusAsync();
            await finalJobDetailsPage.BackAsync();

            // assert
            Assert.AreEqual("Planned", beforeJobStatus);
            Assert.AreEqual("Completed", afterJobStatus);
        }
        finally
        {
            // cleanup
            await _pitstopApp.StopAsync();
        }
    }
}