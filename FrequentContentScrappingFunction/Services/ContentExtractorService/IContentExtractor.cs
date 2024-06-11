namespace FrequentContentScrappingFunction.Services.ContentExtractorService;

public interface IContentExtractor
{
    Task<MemoryStream> ExtractBlockToMemoryStreamAsync(string htmlContent, string blockNode);
    public int GetHighestItemIndex(IEnumerable<string> links);
}