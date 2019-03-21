using OpenQA.Selenium;

namespace Pitstop.UITest.PageModel.Pages
{
    /// <summary>
    /// Base class for all the pages.
    /// </summary>
    public class PitstopPage
    {
        public string Title { get; }
        public PitstopApp Pitstop { get; }

        public IWebDriver WebDriver => Pitstop.WebDriver;


        /// <summary>
        /// Initialize a new PitstopPage instance.
        /// </summary>
        /// <param name="title">The title on the page. This is the text shown as standard title on the page (not the browser window-title!).</param>
        /// <param name="pitstop">The WebApp instance used for the test.</param>
        public PitstopPage(string title, PitstopApp pitstop)
        {
            Title = title;
            Pitstop = pitstop;
        }

        /// <summary>
        /// Indication whether the page with the title of the page is shown.
        /// </summary>
        public bool IsActive()
        {
            var header = WebDriver
                .FindElement(By.Id("PageTitle"));
            return header.Text == Title;
        }

        /// <summary>
        /// Gets the current page with the title of the page being shown.
        /// </summary>
        public PitstopPage GetActivePageTitle(out string pageTitle)
        {
            var header = WebDriver
                .FindElement(By.Id("PageTitle"));
            pageTitle = header.Text;
            return this;
        }
    }
}