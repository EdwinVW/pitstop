using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Pitstop.UITest.PageModel.Pages.WorkshopManagement
{
    /// <summary>
    /// Represents the MaintenanceJob Details page.
    /// </summary>
    public class MaintenanceJobDetailsPage : PitstopPage
    {        
        public MaintenanceJobDetailsPage(PitstopApp pitstop) : base("Workshop Management - details", pitstop)
        {
        }

        public FinishMaintenanceJobPage Complete()
        {
            WebDriver.FindElement(By.Id("CompleteButton")).Click();
            return new FinishMaintenanceJobPage(Pitstop);
        }

        public WorkshopManagementPage Back()
        {
            WebDriver.FindElement(By.Id("BackButton")).Click();
            return new WorkshopManagementPage(Pitstop);
        }

        public MaintenanceJobDetailsPage GetJobStatus(out string status)
        {
            status = WebDriver.FindElement(By.Id("JobStatus")).Text;
            return this;
        }
    }
}