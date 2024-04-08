using BLL.Dtos;

namespace BLL.Interfaces;

public interface IResourcesService
{
    /// <summary>
    /// Retrieves a list of resources related to a given user input text, ranked by relevance.
    /// </summary>
    /// <param name="textUserInput">The user's text input for which to find related resources.</param>
    /// <returns>A list of ResourcesModelDto objects representing the related resources, sorted by relevance.</returns>
    Task<List<ResourcesModelDto>> GetRelatedResourcesAsync(string textUserInput);
}