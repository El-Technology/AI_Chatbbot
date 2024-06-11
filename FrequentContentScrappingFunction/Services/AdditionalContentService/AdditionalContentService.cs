using FrequentContentScrappingFunction.Accessors;
using FrequentContentScrappingFunction.Models;
using FrequentContentScrappingFunction.Services.BlobService;
using FrequentContentScrappingFunction.Services.ScrappingService;

namespace FrequentContentScrappingFunction.Services.AdditionalContentService;

public class AdditionalContentService : IAdditionalContentService
{
    private readonly IAzureBlobStorageService _blobStorageService;
    private readonly IPageConfigurationsAccessor _pageConfigurationAccessor;
    private readonly IScrappingService _scrappingService;

    public AdditionalContentService(IScrappingService scrappingService,
        IAzureBlobStorageService azureBlobStorageService, IPageConfigurationsAccessor pageConfigurationAccessor)
    {
        _scrappingService = scrappingService;
        _blobStorageService = azureBlobStorageService;
        _pageConfigurationAccessor = pageConfigurationAccessor;
    }

    public async Task ParseFrequentContent()
    {
        var itemsToBeParsed = await GetItemsToBeParsed();

        foreach (var item in itemsToBeParsed)
        {
            var appendModel = await _scrappingService.ParseMultiplePagesToMemoryStream(
                item.Configuration.LastParsedItemIndex + 1, item.newMaxIndex, item.Configuration.ResourceLink,
                item.Configuration.Name, item.Configuration.ParsingPageNode);
            await _blobStorageService.AppendToBlobAsync(appendModel.FilepathToAppend, appendModel.MemoryStream);
            await _pageConfigurationAccessor.UpdateLastParsedItemIndex(item.Configuration.Id, item.newMaxIndex);
        }
    }

    public async Task<List<ItemToParse>> GetItemsToBeParsed()
    {
        var configurations = await _pageConfigurationAccessor.GetPageConfigurationsAsync();
        var itemsToParse = new List<ItemToParse>();

        var tasks = configurations.Select(config => Task.Run(async () =>
            {
                var highestIndex =
                    await _scrappingService.GetHighestItemIndexOnPage(config.InitialPageLink, config.InitialPageNode);
                if (highestIndex > config.LastParsedItemIndex) itemsToParse.Add(new ItemToParse(config, highestIndex));
            }))
            .ToList();

        await Task.WhenAll(tasks);
        return itemsToParse;
    }
}

public record ItemToParse(PageConfiguration Configuration, int newMaxIndex);