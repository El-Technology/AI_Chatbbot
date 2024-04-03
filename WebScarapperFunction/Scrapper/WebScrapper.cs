﻿using HtmlAgilityPack;
using PuppeteerSharp;

namespace WebScrapperFunction.Scrapper;

public static class WebScrapper
{
    private const string WebsiteUrl = "https://www.km.qa/pages/SiteMap.aspx";

    public static async Task<List<ResourcesModel>> ParseReferences()
    {
        var options = new LaunchOptions { Headless = true };
        await new BrowserFetcher().DownloadAsync();
        await using var browser = await Puppeteer.LaunchAsync(options);
        await using var page = await browser.NewPageAsync();

        await page.GoToAsync(WebsiteUrl, WaitUntilNavigation.Networkidle2);

        var pageContentHtml = await page.GetContentAsync();
        var parsedContent = ParseLiElements(pageContentHtml);

        return parsedContent;
    }

    public static List<ResourcesModel> ParseLiElements(string htmlContent)
    {
        var parsedData = new List<ResourcesModel>();

        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        var rowDiv = doc.DocumentNode.SelectNodes("//div[contains(@class,'SiteMap-Container')]").FirstOrDefault();

        if (rowDiv == null)
            return parsedData;

        var liNodes = rowDiv.SelectNodes(".//li/a");

        if (liNodes == null)
            return parsedData;

        foreach (var anchorNode in liNodes)
        {
            var title = HtmlEntity.DeEntitize(anchorNode.InnerText.Trim());
            var urlPath = anchorNode.GetAttributeValue("href", string.Empty);

            if (IsValidUrlPath(urlPath, title))
            {
                parsedData.Add(new ResourcesModel
                {
                    Title = title,
                    UrlPath = urlPath
                });
            }
        }

        return parsedData;
    }

    private static bool IsValidUrlPath(string urlPath, string title)
    {
        return !string.IsNullOrEmpty(urlPath) && !string.IsNullOrEmpty(title) &&
               (urlPath.StartsWith("https") || urlPath.StartsWith("/"));
    }
}