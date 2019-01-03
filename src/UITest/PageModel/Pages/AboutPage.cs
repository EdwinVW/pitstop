using OpenQA.Selenium;

namespace Pitstop.UITest.PageModel.Pages
{
    /// <summary>
    /// Represents the About page.
    /// </summary>
    public class AboutPage : PitstopPage
    {
        public AboutPage(PitstopApp pitstop) : base("About Pitstop", pitstop)
        {
        }
    }
}