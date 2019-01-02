using OpenQA.Selenium;

namespace Pitstop.UITest.PageModel.Pages.CustomerManagement
{
    /// <summary>
    /// Represents the CustomerDetails page.
    /// </summary>
    public class CustomerDetailsPage : PitstopPage
    {
        public CustomerDetailsPage(PitstopApp pitstop) : base("Customer Management - details", pitstop)
        {
        }

        public CustomerManagementPage Back()
        {
            WebDriver.FindElement(By.Id("BackButton")).Click();
            return new CustomerManagementPage(Pitstop);
        }
    }
}