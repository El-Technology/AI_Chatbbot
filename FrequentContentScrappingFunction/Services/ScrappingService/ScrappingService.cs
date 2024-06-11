using FrequentContentScrappingFunction.Models;
using FrequentContentScrappingFunction.Services.BrowserService;
using FrequentContentScrappingFunction.Services.ContentExtractorService;
using HtmlAgilityPack;
using PuppeteerSharp;

namespace FrequentContentScrappingFunction.Services.ScrappingService;

public class ScrappingService : IScrappingService
{
    private readonly IBrowserService _browserService;
    private readonly IContentExtractor _contentExtractor;

    public ScrappingService(IBrowserService browserService, IContentExtractor contentExtractor)
    {
        _browserService = browserService;
        _contentExtractor = contentExtractor;
        _browserService.InitializeBrowserAsync().GetAwaiter().GetResult();
    }

    public async Task<AppendModel> ParseMultiplePagesToMemoryStream(int startId, int endId, string resourceLink, string filepathToSaveName, string blockNode)
    {
        var tasks = new List<Task<MemoryStream>>();

        for (var i = startId; i <= endId; i++)
        {
            var currentId = i;
            tasks.Add(Task.Run(async () =>
            {
                await using var page = await _browserService.GetPageAsync();
                await NavigateToPageAsync(page, $"{resourceLink}{currentId}");
                var pageContentHtml = await page.GetContentAsync();
                var stream = await _contentExtractor.ExtractBlockToMemoryStreamAsync(pageContentHtml, blockNode);
                Console.WriteLine($"Extracted: {currentId}");
                return stream;
            }));
        }

        var memoryStreams = await Task.WhenAll(tasks);

        // Combine all MemoryStreams into one
        var combinedStream = new MemoryStream();
        foreach (var ms in memoryStreams)
        {
            ms.Position = 0;
            await ms.CopyToAsync(combinedStream);
            await ms.DisposeAsync();
        }

        combinedStream.Position = 0;

        return new AppendModel
        {
            FilepathToAppend = filepathToSaveName,
            MemoryStream = combinedStream
        };
    }

    public async Task<int> GetHighestItemIndexOnPage(string pageUrl, string upperNodeXpath)
    {
        await using var page = await _browserService.GetPageAsync();
        await NavigateToPageAsync(page, pageUrl);
        var pageContentHtml = await page.GetContentAsync();

        var doc = new HtmlDocument();
        doc.LoadHtml(pageContentHtml);

        var mediaBodies = doc.DocumentNode.SelectNodes(upperNodeXpath);

        if (mediaBodies is null)
        {
            Console.WriteLine("No media bodies found");
            return 0;
        }

        var links = mediaBodies.Select(node => node.GetAttributeValue("href", string.Empty))
            .Where(href => !string.IsNullOrEmpty(href)).ToList();

        return _contentExtractor.GetHighestItemIndex(links);
    }

    private static async Task NavigateToPageAsync(IPage page, string url)
    {
        await page.GoToAsync(url, new NavigationOptions
        {
            Timeout = 0,
            WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
        });
    }
}