namespace Pitstop.UITest.PageModel.Pages;

/// <summary>
/// Base class for all the pages.
/// </summary>
public class PitstopPage
{
    public string Title { get; }
    public PitstopApp Pitstop { get; }

    public IPage Page => Pitstop.Page;


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
    public async Task<bool> IsActiveAsync()
    {
        var header = await Page.QuerySelectorAsync("#PageTitle");
        if (header == null) return false;
        var text = await header.TextContentAsync();
        return text == Title;
    }

    /// <summary>
    /// Gets the current page with the title of the page being shown.
    /// </summary>
    public async Task<(PitstopPage page, string pageTitle)> GetActivePageTitleAsync()
    {
        var header = await Page.QuerySelectorAsync("#PageTitle");
        var pageTitle = header != null ? await header.TextContentAsync() : "";
        return (this, pageTitle);
    }
}