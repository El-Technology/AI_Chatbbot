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

    public async Task<List<ResourcesModel>> GetRelatedResources(Vector requestVector)
    {
        return await _context.ResourcesModels
            .OrderBy(x => x.Embedding!.CosineDistance(requestVector))
            .Take(3)
            .ToListAsync();
    }
}