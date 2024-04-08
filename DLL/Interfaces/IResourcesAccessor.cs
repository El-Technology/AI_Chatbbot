using DLL.Models;
using Pgvector;

namespace DLL.Interfaces;

public interface IResourcesAccessor
{
    Task<List<ResourcesModel>> GetRelatedResources(Vector requestVector);
}