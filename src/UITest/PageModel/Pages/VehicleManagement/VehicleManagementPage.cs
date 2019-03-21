using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Pitstop.UITest.PageModel.Pages.VehicleManagement
{
    /// <summary>
    /// Represents the VehicleManagement page.
    /// </summary>
    public class VehicleManagementPage : PitstopPage
    {
        public VehicleManagementPage(PitstopApp pitstop) : base("Vehicle Management - overview", pitstop)
        {
        }

        public RegisterVehiclePage RegisterVehicle()
        {
            WebDriver.FindElement(By.Id("RegisterVehicleButton")).Click();
            return new RegisterVehiclePage(Pitstop);
        }

        public VehicleDetailsPage SelectVehicle(string licenseNumber)
        {
            WebDriver
                .FindElement(By.XPath($"//td[contains(text(),'{licenseNumber}')]"))
                .Click();
            return new VehicleDetailsPage(Pitstop); 
        }
    }
}