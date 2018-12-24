using System;
using Xunit;
using Xunit.Abstractions;
using Pitstop.UITest.PageModel;

namespace Pitstop.UITest
{
    public class ScenarioTests
    {
        private readonly ITestOutputHelper _output;

        public ScenarioTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void End_To_End()
        {
            // arrange
            Guid testrunId = Guid.NewGuid();
            PitstopApp pitstop = new PitstopApp(testrunId, TestConstants.PitstopStartUrl);
            var homePage = pitstop.Start();

            // act
            pitstop.Menu
                .CustomerManagement()
                .RegisterCustomer()
                .Cancel()
                .RegisterCustomer()
                .FillCustomerDetails($"TestCustomer {testrunId}", "Verzonnenstraat 21", "Uitdeduimerveen", "1234 AZ", "+31612345678", "tc@test.com")
                .Submit()
                .SelectCustomer($"TestCustomer {testrunId}")
                .Back();

            pitstop.Menu
                .VehicleManagement()
                .RegisterVehicle()
                .Cancel()
                .RegisterVehicle()
                .FillVehicleDetails($"Vehicle {testrunId}", "Testla", "Model T", $"TestCustomer {testrunId}")
                .Submit()
                .SelectVehicle($"Vehicle {testrunId}")
                .Back();

            // cleanup
            pitstop.Stop();
        }
    }
}
