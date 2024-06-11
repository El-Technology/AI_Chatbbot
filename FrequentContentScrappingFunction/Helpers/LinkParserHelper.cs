using System.Text.RegularExpressions;

namespace FrequentContentScrappingFunction.Helpers;

public static class LinkParserHelper
{
    private const string Pattern = @"ItemI[Dd]=(\d+)";

    public static int GetHighestItemId(List<string> urls)
    {
        var highestItemId = 0;

        foreach (var url in urls)
        {
            var match = Regex.Match(url, Pattern);

            if (!match.Success)
                continue;

            var itemId = int.Parse(match.Groups[1].Value);
            if (itemId > highestItemId) highestItemId = itemId;
        }

        return highestItemId;
    }
}