namespace Pitstop.UITest.PageModel;

/// <summary>
/// Represents the Pitstop web-application.
/// </summary>
public class PitstopApp : IAsyncDisposable
{
    private bool _disposed = false;
    private readonly IPlaywright _playwright;
    private readonly IBrowser _browser;
    private readonly IPage _page;
    private readonly Uri _startUrl;
    private readonly MainMenu _menu;

    public MainMenu Menu
    {
        get
        {
            return _menu;
        }
    }

    public IPage Page
    {
        get
        {
            return _page;
        }
    }


    /// <summary>
    /// Initialize a new Pitstop instance.
    /// </summary>
    /// <param name="testrunId">The unique test-run Id.</param>
    /// <param name="startUrl">The Url to start.</param>
    /// <param name="playwright">The Playwright instance.</param>
    /// <param name="browser">The Browser instance.</param>
    /// <param name="page">The Page instance.</param>
    private PitstopApp(string testrunId, Uri startUrl, IPlaywright playwright, IBrowser browser, IPage page)
    {
        _playwright = playwright;
        _browser = browser;
        _page = page;
        _startUrl = startUrl;
        _menu = new MainMenu(this);
    }

    /// <summary>
    /// Creates a new Pitstop instance asynchronously.
    /// </summary>
    /// <param name="testrunId">The unique test-run Id.</param>
    /// <param name="startUrl">The Url to start.</param>
    /// <returns>A new PitstopApp instance.</returns>
    public static async Task<PitstopApp> CreateAsync(string testrunId, Uri startUrl)
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Channel = "msedge",
            Headless = false
        });
        var page = await browser.NewPageAsync();
        await page.SetViewportSizeAsync(1920, 1080);
        return new PitstopApp(testrunId, startUrl, playwright, browser, page);
    }

    /// <summary>
    /// Open the Pitstop application.
    /// </summary>
    /// <returns>An initialized <see cref="HomePage"/> instance.</returns>
    public async Task<HomePage> StartAsync()
    {
        await _page.GotoAsync(_startUrl.ToString());
        return CreatePage<HomePage>();
    }

    /// <summary>
    /// Stop the WebApp instance and close the browser.
    /// </summary>
    public async Task StopAsync()
    {
        await DisposeAsync();
    }

    /// <summary>
    /// Take a screenshot and save it in the specified file.
    /// </summary>
    /// <param name="outputFilename">The name of the file to output.</param>
    public async Task TakeScreenshotAsync(string outputFilename)
    {
        await _page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = outputFilename
        });
    }

    /// <summary>
    /// Create a new Pitstop page.
    /// </summary>
    /// <typeparam name="T">The type of the page to create.</typeparam>
    public T CreatePage<T>() where T : PitstopPage
    {
        return Activator.CreateInstance(typeof(T), new object[] { this }) as T;
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
            }
            if (_playwright != null)
            {
                _playwright.Dispose();
            }
        }

        _disposed = true;
    }
}