using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Pitstop.UITest.PageModel.Pages
{
    /// <summary>
    /// Represents the WorkshopManagement page.
    /// </summary>
    public class WorkshopManagementPage : PitstopPage
    {
        public WorkshopManagementPage(PitstopApp pitstop) : base("Workshop Management", pitstop)
        {
        }
    }
}