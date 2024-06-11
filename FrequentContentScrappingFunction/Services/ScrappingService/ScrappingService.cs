using FrequentContentScrappingFunction.Helpers;
using FrequentContentScrappingFunction.Models;
using HtmlAgilityPack;
using PuppeteerSharp;
using System.Text;

namespace FrequentContentScrappingFunction.Services.ScrappingService;

public class ScrappingService : IScrappingService
{
    public async Task<AppendModel> ParseMultiplePagesToMemoryStream(int startId, int endId, string resourceLink,
        string filepathToSaveName, string blockNode)
    {
        var tasks = new List<Task>();

        var options = new LaunchOptions { Headless = true };
        await new BrowserFetcher().DownloadAsync();
        await using var browser = await Puppeteer.LaunchAsync(options);
        await using var page = await browser.NewPageAsync();

        var ms = new MemoryStream();

        for (var i = startId; i <= endId; i++)
        {
            var currentId = i;
            tasks.Add(Task.Run(async () =>
            {
                await using var page = await browser.NewPageAsync();
                await page.GoToAsync(resourceLink + $"{currentId}", new NavigationOptions
                {
                    Timeout = 0,
                    WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
                });
                var pageContentHtml = await page.GetContentAsync();
                var stream = await ExtractBlockToMemoryStreamAsync(pageContentHtml, blockNode);
                await stream.CopyToAsync(ms);
                Console.WriteLine($"Extracted: {currentId}");
            }));
        }

        await Task.WhenAll(tasks);
        await browser.CloseAsync();
        return new AppendModel
        {
            FilepathToAppend = filepathToSaveName,
            MemoryStream = ms
        };
    }

    public async Task<MemoryStream> ExtractBlockToMemoryStreamAsync(string htmlContent, string blockNode)
    {
        if (string.IsNullOrEmpty(htmlContent))
            return new MemoryStream();

        try
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var textNodes = doc.DocumentNode.SelectNodes(blockNode);

            if (textNodes is null)
            {
                Console.WriteLine($"\t Skipped block node: {blockNode}");
                return new MemoryStream();
            }

            var extractedContent = new StringBuilder();
            foreach (var node in textNodes)
            {
                extractedContent.AppendLine(node.InnerHtml);
            }

            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            await writer.WriteAsync(extractedContent.ToString());
            await writer.FlushAsync();
            memoryStream.Position = 0;

            return memoryStream;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extracting blocks: {ex.Message}");
            return new MemoryStream();
        }
    }

    private static async Task<IPage> GetBrowserPageAsync()
    {
        var options = new LaunchOptions { Headless = true };
        await new BrowserFetcher().DownloadAsync();
        var browser = await Puppeteer.LaunchAsync(options);
        var page = await browser.NewPageAsync();

        return page;
    }

    public async Task<int> GetHighestItemIndexOnPage(string pageUrl, string upperNodeXpath)
    {
        var page = await GetBrowserPageAsync();

        await page.GoToAsync(pageUrl, new NavigationOptions()
        {
            Timeout = 0,
            WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
        });

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

        var highestId = LinkParserHelper.GetHighestItemId(links);

        return highestId;
    }
}