using System.Text;
using BLL.Dtos;

namespace BLL.Helpers;

public static class BotMarkdownHelper
{
    private const string HrMarkdown = "---";
    private const string WebUrl = "https://km.qa";
    public static string GetResourceLinksMarkdown(List<ResourcesModelDto> resources, bool shouldHaveHr)
    {
        var sb = new StringBuilder();

        if (!resources.Any())
            return sb.ToString();

        if (shouldHaveHr)
            sb.AppendLine(HrMarkdown);

        foreach (var resource in resources)
        {
            if (resource.UrlPath == null || !resource.UrlPath.StartsWith("/"))
                resource.UrlPath = WebUrl + resource.UrlPath;

            sb.AppendLine($"[{resource.Title}]({resource.UrlPath})");
        }

        return sb.ToString();
    }
}