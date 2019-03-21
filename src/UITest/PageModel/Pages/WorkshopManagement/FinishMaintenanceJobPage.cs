using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Pitstop.UITest.PageModel.Pages.WorkshopManagement
{
    /// <summary>
    /// Represents the Finish MaintenanceJob page.
    /// </summary>
    public class FinishMaintenanceJobPage : PitstopPage
    {        
        public FinishMaintenanceJobPage(PitstopApp pitstop) : base("Workshop Management - finish maintenance job", pitstop)
        {
        }

        public FinishMaintenanceJobPage FillJobDetails(string actualStartTime, string actualEndTime, string notes)
        {
            var startTimeBox = WebDriver.FindElement(By.Name("ActualStartTime"));
            startTimeBox.Clear();
            startTimeBox.SendKeys(actualStartTime);

            var endTimeBox = WebDriver.FindElement(By.Name("ActualEndTime"));
            endTimeBox.Clear();
            endTimeBox.SendKeys(actualEndTime);

            WebDriver.FindElement(By.Name("Notes")).SendKeys(notes);
            
            return this;
        }

        public MaintenanceJobDetailsPage Complete()
        {
            WebDriver.FindElement(By.Id("CompleteButton")).Click();
            return new MaintenanceJobDetailsPage(Pitstop);
        }

        public MaintenanceJobDetailsPage Cancel()
        {
            WebDriver.FindElement(By.Id("CancelButton")).Click();
            return new MaintenanceJobDetailsPage(Pitstop);
        }
    }
}