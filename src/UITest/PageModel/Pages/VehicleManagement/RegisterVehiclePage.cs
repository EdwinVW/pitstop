using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Pitstop.UITest.PageModel.Pages.VehicleManagement
{
    /// <summary>
    /// Represents the RegisterVehicle page.
    /// </summary>
    public class RegisterVehiclePage : PitstopPage
    {
        public RegisterVehiclePage(PitstopApp pitstop) : base("Vehicle Management - register vehicle", pitstop)
        {
        }

        public RegisterVehiclePage FillVehicleDetails(string licenseNumber, string brand, string type, string owner)
        {
            WebDriver.FindElement(By.Name("Vehicle.LicenseNumber")).SendKeys(licenseNumber);
            WebDriver.FindElement(By.Name("Vehicle.Brand")).SendKeys(brand);
            WebDriver.FindElement(By.Name("Vehicle.Type")).SendKeys(type);
            SelectElement select = new SelectElement(WebDriver.FindElement(By.Id("SelectedCustomerId")));
            select.SelectByText(owner);
            return this;
        }

        public VehicleManagementPage Submit()
        {
            WebDriver.FindElement(By.Id("SubmitButton")).Click();
            return new VehicleManagementPage(Pitstop);
        }

        public VehicleManagementPage Cancel()
        {
            WebDriver.FindElement(By.Id("CancelButton")).Click();
            return new VehicleManagementPage(Pitstop);
        }
    }
}