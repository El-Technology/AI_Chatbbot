using System.Text;
using BLL.Dtos;

namespace BLL.Helpers;

public static class BotMarkdownHelper
{
    private const string WebUrl = "https://km.qa";
    public static string GetResourceLinksMarkdownAsync(List<ResourcesModelDto> resources)
    {
        var sb = new StringBuilder();

        if (!resources.Any())
        {
            return sb.ToString();
        }

        foreach (var resource in resources)
        {
            if (resource.UrlPath == null || !resource.UrlPath.StartsWith("/"))
                continue;

            resource.UrlPath = WebUrl + resource.UrlPath;
            sb.AppendLine($"[{resource.Title}]({resource.UrlPath})");
        }

        return sb.ToString();
    }
}