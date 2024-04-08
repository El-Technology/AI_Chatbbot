using DLL.Models;
using Pgvector;

namespace DLL.Interfaces;

public interface IResourcesAccessor
{
    /// <summary>
    /// Retrieves a list of resources related to a given request vector, ranked by cosine similarity and limited to a specified count.
    /// </summary>
    /// <param name="requestVector">The vector embedding representing the user's request.</param>
    /// <param name="responseCount">The maximum number of related resources to return.</param>
    /// <returns>A list of ResourcesModel objects representing the related resources, sorted by relevance.</returns>
    Task<List<ResourcesModel>> GetRelatedResources(Vector requestVector, int responseCount);

    /// <summary>
    /// Retrieves a list of resources related to a given request vector, filtered by a maximum cosine distance and ranked by relevance.
    /// </summary>
    /// <param name="requestVector">The vector embedding representing the user's request.</param>
    /// <param name="vectorDistance">The maximum cosine distance allowed for a resource to be considered related.</param>
    /// <returns>A list of ResourcesModel objects representing the related resources, sorted by relevance.</returns>
    Task<List<ResourcesModel>> GetRelatedResources(Vector requestVector, double vectorDistance);
}