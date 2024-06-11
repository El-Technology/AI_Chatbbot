using PuppeteerSharp;

namespace FrequentContentScrappingFunction.Services.BrowserService;

public class BrowserService : IBrowserService
{
    private IBrowser _browser;

    public async Task InitializeBrowserAsync()
    {
        var options = new LaunchOptions { Headless = true };
        await new BrowserFetcher().DownloadAsync();
        _browser = await Puppeteer.LaunchAsync(options);
    }

    public async Task<IPage> GetPageAsync()
    {
        return await _browser.NewPageAsync();
    }
}