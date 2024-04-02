using HtmlAgilityPack;

namespace WebScrapperFunction.Scrapper;

public static class WebScrapper
{
    private const string WebsiteUrl = "https://learn.microsoft.com/en-us/azure/azure-functions/functions-triggers-bindings?tabs=isolated-process%2Cpython-v2&pivots=programming-language-csharp"; //"https://www.km.qa/pages/SiteMap.aspx";
    private static readonly HttpClient HttpClient = new();

    public static async Task<List<ResourcesModel>> ParseReferences()
    {
        var httpResponseMessage = await HttpClient.GetAsync(WebsiteUrl);
        
        EnsureSuccessStatusCode(httpResponseMessage);

        var pageContentHtml = await httpResponseMessage.Content.ReadAsStringAsync();

        var parsedContent = ParseLiElements(pageContentHtml);

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
            var title = anchorNode?.InnerText.Trim() ?? string.Empty;

            var urlPath = anchorNode?.GetAttributeValue("href", string.Empty) ?? string.Empty;


            if(!string.IsNullOrEmpty(urlPath) && !string.IsNullOrEmpty(title))
                parsedData.Add(new ResourcesModel
                {
                    Title = title,
                    UrlPath = urlPath
                });
        }

        return parsedData;
    }
}