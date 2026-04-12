namespace Pitstop.UITest;

[TestClass]
public class ScenarioTests
{
    string _testrunId;
    PitstopApp _pitstopApp;
    string _licenseNumber;
    string _customerName;
    string _address;
    string _city;
    string _postalCode;
    string _phoneNumber;
    string _email;
    string _vehicleBrand;
    string _vehicleModel;
    string _jobDescription;

    [TestInitialize]
    public async Task Initialize()
    {
        await DatabaseCleaner.ClearAllAsync();

        _testrunId = Guid.NewGuid().ToString("N");
        _licenseNumber = TestDataPrimitives.GenerateRandomLicenseNumber();
        _customerName = $"{TestDataPrimitives.RandomName()} {_testrunId}";
        _address = TestDataPrimitives.RandomAddress();
        _city = TestDataPrimitives.RandomCity();
        _postalCode = TestDataPrimitives.RandomPostalCode();
        _phoneNumber = TestDataPrimitives.RandomPhoneNumber();
        _email = TestDataPrimitives.RandomEmailAddress();
        var carType = TestDataPrimitives.RandomCar();
        _vehicleBrand = carType.Brand;
        _vehicleModel = carType.Model;
        _jobDescription = $"{TestDataPrimitives.RandomDescription()} {_testrunId}";

        _pitstopApp = await PitstopApp.CreateAsync(_testrunId, TestConstants.PitstopStartUrl);
        await _pitstopApp.StartAsync();
    }

    [TestMethod]
    public async Task Full_MaintenanceJob_Process()
    {
        try
        {
            // Register Customer
            var customerManagementPage = await _pitstopApp.Menu.CustomerManagementAsync();
            var registerCustomerPage = await customerManagementPage.RegisterCustomerAsync();
            var cancelledPage = await registerCustomerPage.CancelAsync();
            var registerCustomerPage2 = await cancelledPage.RegisterCustomerAsync();
            var filledPage = await registerCustomerPage2.FillCustomerDetailsAsync(
                _customerName, _address, _city, _postalCode, _phoneNumber, _email);
            var submittedPage = await filledPage.SubmitAsync();
            var customerDetailsPage = await submittedPage.SelectCustomerAsync(_customerName);
            await customerDetailsPage.BackAsync();
    
            // Register Vehicle
            var vehicleManagementPage = await _pitstopApp.Menu.VehicleManagementAsync();
            var registerVehiclePage = await vehicleManagementPage.RegisterVehicleAsync();
            var cancelledVehiclePage = await registerVehiclePage.CancelAsync();
            var registerVehiclePage2 = await cancelledVehiclePage.RegisterVehicleAsync();
            var filledVehiclePage = await registerVehiclePage2.FillVehicleDetailsAsync(_licenseNumber, _vehicleBrand, _vehicleModel, _customerName);
            var submittedVehiclePage = await filledVehiclePage.SubmitAsync();
            var vehicleDetailsPage = await submittedVehiclePage.SelectVehicleAsync(_licenseNumber);
            await vehicleDetailsPage.BackAsync();

            // Register MaintenanceJob
            var workshopManagementPage = await _pitstopApp.Menu.WorkshopManagementAsync();
            var registerMaintenanceJobPage = await workshopManagementPage.RegisterMaintenanceJobAsync();
            var cancelledJobPage = await registerMaintenanceJobPage.CancelAsync();
            var registerMaintenanceJobPage2 = await cancelledJobPage.RegisterMaintenanceJobAsync();
            var filledJobPage = await registerMaintenanceJobPage2.FillJobDetailsAsync("08:00", "12:00", _jobDescription, _licenseNumber);
            var submittedJobPage = await filledJobPage.SubmitAsync();
            var jobDetailsPage = await submittedJobPage.SelectMaintenanceJobAsync(_jobDescription);
            await jobDetailsPage.BackAsync();

            // Complete MaintenanceJob
            var workshopManagementPage2 = await _pitstopApp.Menu.WorkshopManagementAsync();
            var jobDetailsPage2 = await workshopManagementPage2.SelectMaintenanceJobAsync(_jobDescription);
            var beforeJobStatus = await jobDetailsPage2.GetJobStatusAsync();
            var finishJobPage = await jobDetailsPage2.CompleteAsync();
            var filledFinishJobPage = await finishJobPage.FillJobDetailsAsync("08:00", "11:00", $"Mechanic notes {_testrunId}");
            var completedJobPage = await filledFinishJobPage.CompleteAsync();
            await completedJobPage.BackAsync();

            // Select MaintenanceJob
            var workshopManagementPage3 = await _pitstopApp.Menu.WorkshopManagementAsync();
            var finalJobDetailsPage = await workshopManagementPage3.SelectMaintenanceJobAsync(_jobDescription);
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