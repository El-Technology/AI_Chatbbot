using BLL.Dtos;
using System.Text;

namespace BLL.Helpers;

public static class BotMarkdownHelper
{
    private const string HrMarkdown = "---";
    private const string WebUrl = "https://km.qa";

    /// <summary>
    /// Generates a Markdown formatted string containing links to related resources.
    /// </summary>
    /// <param name="resources">A list of ResourcesModelDto objects representing the resources to link.</param>
    /// <param name="shouldHaveHr">A flag indicating whether to include a horizontal rule (HR) before the list (optional).</param>
    /// <returns>A string containing the Markdown formatted resource links, or an empty string if no resources are provided.</returns>
    public static string GetResourceLinksMarkdown(List<ResourcesModelDto> resources, bool shouldHaveHr)
    {
        var sb = new StringBuilder();

        if (!resources.Any())
            return string.Empty;

        sb.AppendLine("\n");

        if (shouldHaveHr)
            sb.AppendLine(HrMarkdown);

        foreach (var resource in resources)
        {
            if (resource.UrlPath == null || resource.UrlPath.StartsWith("/"))
                resource.UrlPath = WebUrl + resource.UrlPath;

            sb.AppendLine($"[{resource.Title}]({resource.UrlPath})");
        }

        return sb.ToString();
    }
}