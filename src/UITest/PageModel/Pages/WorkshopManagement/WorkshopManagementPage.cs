using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Pitstop.UITest.PageModel.Pages.WorkshopManagement
{
    /// <summary>
    /// Represents the WorkshopManagement page.
    /// </summary>
    public class WorkshopManagementPage : PitstopPage
    {
        public WorkshopManagementPage(PitstopApp pitstop) : base("Workshop Management - overview", pitstop)
        {
        }

        public RegisterMaintenanceJobPage RegisterMaintenanceJob()
        {
            WebDriver.FindElement(By.Id("RegisterMaintenanceJobButton")).Click();
            return new RegisterMaintenanceJobPage(Pitstop);
        }

        public MaintenanceJobDetailsPage SelectMaintenanceJob(string jobDescription)
        {
            WebDriver
                .FindElement(By.XPath($"//td[contains(text(),'{jobDescription}')]"))
                .Click();
            return new MaintenanceJobDetailsPage(Pitstop); 
        }
    }
}