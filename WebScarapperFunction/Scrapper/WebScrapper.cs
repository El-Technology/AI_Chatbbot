using HtmlAgilityPack;
using PuppeteerSharp;

namespace WebScrapperFunction.Scrapper;

public static class WebScrapper
{
    private const string WebsiteUrl = "https://www.km.qa/pages/SiteMap.aspx";

    public static async Task<List<ResourcesModel>> ParseReferences()
    {
        var options = new LaunchOptions { Headless = true };
        await using var browser = await Puppeteer.LaunchAsync(options);
        await using var page = await browser.NewPageAsync();

        await page.GoToAsync(WebsiteUrl, WaitUntilNavigation.Networkidle2);

        var parsedContent = await page.EvaluateFunctionAsync<List<ResourcesModel>>(@"
                () => {
                    const parsedData = [];
                    document.querySelectorAll('li > a').forEach(anchorNode => {
                        const title = anchorNode.innerText.trim();
                        const urlPath = anchorNode.getAttribute('href') || '';
                        if (isValidUrlPath(urlPath, title)) {
                            parsedData.push({ Title: title, UrlPath: urlPath });
                        }
                    });
                    return parsedData;
                }
            ");

        return parsedContent;
    }

    private static void EnsureSuccessStatusCode(HttpResponseMessage httpResponseMessage)
    {
        if(httpResponseMessage.IsSuccessStatusCode)
            return;

        var statusCode = httpResponseMessage.StatusCode;
        var httpContent = httpResponseMessage.Content.ReadAsStringAsync();
        var reasonPhrase = httpResponseMessage.ReasonPhrase;

        throw new HttpRequestException(
            $"Status code: {(int)statusCode}-{statusCode}. Reason phrase: ({reasonPhrase}. Content: ({httpContent})");
    }

    public static List<ResourcesModel> ParseLiElements(string htmlContent)
    {
        var parsedData = new List<ResourcesModel>();

        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        var liNodes = doc.DocumentNode.SelectNodes("//li");

        if (liNodes == null) 
            return parsedData;

        foreach (var liNode in liNodes)
        {
            var anchorNode = liNode.SelectSingleNode(".//a");
            if(anchorNode == null) 
                continue;
            var title = HtmlEntity.DeEntitize(anchorNode.InnerText.Trim());

            var urlPath = anchorNode?.GetAttributeValue("href", string.Empty) ?? string.Empty;


            if(IsValidUrlPath(urlPath, title))
                parsedData.Add(new ResourcesModel
                {
                    Title = title,
                    UrlPath = urlPath
                });
        }

        return parsedData;
    }

    private static bool IsValidUrlPath(string urlPath, string title)
    {
        return !string.IsNullOrEmpty(urlPath) && !string.IsNullOrEmpty(title) &&
               (urlPath.StartsWith("https") || urlPath.StartsWith("/"));
    }
}