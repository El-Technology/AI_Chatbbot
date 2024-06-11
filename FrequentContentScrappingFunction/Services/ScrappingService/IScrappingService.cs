using FrequentContentScrappingFunction.Models;

namespace FrequentContentScrappingFunction.Services.ScrappingService;

public interface IScrappingService
{
    Task<int> GetHighestItemIndexOnPage(string pageUrl, string upperNodeXpath);

    Task<AppendModel> ParseMultiplePagesToMemoryStream(int startId, int endId, string resourceLink,
        string filepathToSaveName, string blockNode);
}