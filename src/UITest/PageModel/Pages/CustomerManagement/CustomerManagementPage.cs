using OpenQA.Selenium;

namespace Pitstop.UITest.PageModel.Pages.CustomerManagement
{
    /// <summary>
    /// Represents the CustomerManagement page.
    /// </summary>
    public class CustomerManagementPage : PitstopPage
    {
        public CustomerManagementPage(PitstopApp pitstop) : base("Customer Management - overview", pitstop)
        {
        }

        public RegisterCustomerPage RegisterCustomer()
        {
            WebDriver.FindElement(By.Id("RegisterCustomerButton")).Click();
            return new RegisterCustomerPage(Pitstop);
        }

        public CustomerDetailsPage SelectCustomer(string customerName)
        {
            WebDriver
                .FindElement(By.XPath($"//td[contains(text(),'{customerName}')]"))
                .Click();
            return new CustomerDetailsPage(Pitstop); 
        }
    }
}