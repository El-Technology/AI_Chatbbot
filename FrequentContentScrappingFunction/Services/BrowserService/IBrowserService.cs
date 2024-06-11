using PuppeteerSharp;

namespace FrequentContentScrappingFunction.Services.BrowserService;

public interface IBrowserService
{
    Task<IPage> GetPageAsync();
    Task InitializeBrowserAsync();
}