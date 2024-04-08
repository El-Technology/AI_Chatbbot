using DLL.Context;
using DLL.Interfaces;
using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace DLL.Accessors;

public class ResourcesAccessor : IResourcesAccessor
{
    private readonly AIChatbotDbContext _context;

    public ResourcesAccessor(AIChatbotDbContext context)
    {
        _context = context;
    }

    ///<inheritdoc cref="IResourcesAccessor.GetRelatedResources(Vector, int)"/>
    public async Task<List<ResourcesModel>> GetRelatedResources(Vector requestVector, int responseCount)
    {
        return await _context.ResourcesModels
            .OrderBy(x => x.Embedding!.CosineDistance(requestVector))
            .Take(responseCount)
            .ToListAsync();
    }

    ///<inheritdoc cref="IResourcesAccessor.GetRelatedResources(Vector, double)"/>
    public async Task<List<ResourcesModel>> GetRelatedResources(Vector requestVector, double vectorDistance)
    {
        return await _context.ResourcesModels
            .Where(x => x.Embedding!.CosineDistance(requestVector) < vectorDistance)
            .OrderBy(x => x.Embedding!.CosineDistance(requestVector))
            .ToListAsync();
    }
}