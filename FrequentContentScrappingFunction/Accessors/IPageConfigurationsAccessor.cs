using FrequentContentScrappingFunction.Models;

namespace FrequentContentScrappingFunction.Accessors;

public interface IPageConfigurationsAccessor
{
    Task<List<PageConfiguration>> GetPageConfigurationsAsync();
    Task UpdateLastParsedItemIndex(Guid pageConfigId, int newIndex);
}