using DLL.Models;
using Pgvector;

namespace DLL.Interfaces;

public interface IResourcesAccessor
{
    Task<List<ResourcesModel>> GetRelatedResources(Vector requestVector, int responseCount);
    Task<List<ResourcesModel>> GetRelatedResources(Vector requestVector, double vectorDistance);
}