using Microsoft.EntityFrameworkCore;
using WebScrapperFunction.Models;
using WebScrapperFunction.OpenAIEmbeddingClient;

namespace WebScrapperFunction.Accessors;

public class ResourcesModelAccessor : IResourcesModelAccessor
{
    private readonly AIChatbotDbContext _context;
    private readonly IOpenAIClientService _openAIClientService;

    public ResourcesModelAccessor(AIChatbotDbContext context, IOpenAIClientService openAIClientService)
    {
        _context = context;
        _openAIClientService = openAIClientService;
    }

    public async Task UpdateResources(List<ResourcesModel> parsedResources)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingResources = await _context.ResourcesModels.ToListAsync();

            CompareResources(existingResources, parsedResources, out var resToRemove, out var resToAdd);

            if (resToRemove.Any())
                _context.ResourcesModels.RemoveRange(resToRemove);

            if (resToAdd.Any())
            {
                await _openAIClientService.EmbedResCollectionAsync(resToAdd);
                await _context.ResourcesModels.AddRangeAsync(resToAdd);
            }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static void CompareResources(
        IReadOnlyCollection<ResourcesModel> existingResources,
        IReadOnlyCollection<ResourcesModel> parsedResources,
        out List<ResourcesModel> resToRemove,
        out List<ResourcesModel> resToAdd)
    {
        resToRemove = new List<ResourcesModel>();
        resToAdd = new List<ResourcesModel>();

        var parsedTitleUrlPairs = parsedResources.Select(r => (r.Title, r.UrlPath)).ToHashSet();
        var existingTitleUrlPairs = existingResources.Select(r => (r.Title, r.UrlPath)).ToHashSet();

        resToRemove = existingResources
            .Where(r => !parsedTitleUrlPairs.Contains((r.Title, r.UrlPath)))
            .ToList();

        resToAdd = parsedResources
            .Where(r => !existingTitleUrlPairs.Contains((r.Title, r.UrlPath)))
            .ToList();
    }
}