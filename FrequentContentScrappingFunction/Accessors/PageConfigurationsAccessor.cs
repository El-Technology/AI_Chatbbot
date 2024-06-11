using FrequentContentScrappingFunction.Models;
using Microsoft.EntityFrameworkCore;

namespace FrequentContentScrappingFunction.Accessors;

public class PageConfigurationsAccessor : IPageConfigurationsAccessor
{
    private readonly AIChatbotDbContext _context;

    public PageConfigurationsAccessor(AIChatbotDbContext context)
    {
        _context = context;
    }

    public Task<List<PageConfiguration>> GetPageConfigurationsAsync()
    {
        return _context.PageConfigurations.ToListAsync();
    }

    public async Task UpdateLastParsedItemIndex(Guid pageConfigId, int newIndex)
    {
        var pageConfigToUpdate = await GetPageConfigurationByIdAsync(pageConfigId)
                                 ?? throw new Exception("Page configuration not found");

        pageConfigToUpdate.LastParsedItemIndex = newIndex;
        await _context.SaveChangesAsync();
    }

    public ValueTask<PageConfiguration?> GetPageConfigurationByIdAsync(Guid id)
    {
        return _context.PageConfigurations.FindAsync(id);
    }
}