using HtmlAgilityPack;
using System.Text;
using FrequentContentScrappingFunction.Helpers;
using FrequentContentScrappingFunction.Services.ContentExtractorService;

public class ContentExtractor : IContentExtractor
{
    public async Task<MemoryStream> ExtractBlockToMemoryStreamAsync(string htmlContent, string blockNode)
    {
        if (string.IsNullOrEmpty(htmlContent))
            return new MemoryStream();

        try
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var textNodes = doc.DocumentNode.SelectNodes(blockNode);

            if (textNodes == null)
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
            using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
            {
                await writer.WriteAsync(extractedContent.ToString());
                await writer.FlushAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extracting blocks: {ex.Message}");
            return new MemoryStream();
        }
    }

    public int GetHighestItemIndex(IEnumerable<string> links)
    {
        return LinkParserHelper.GetHighestItemId(links.ToList());
    }
}