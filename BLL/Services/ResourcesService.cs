using BLL.Dtos;
using BLL.Helpers;
using BLL.Interfaces;
using DLL.Interfaces;
using DLL.Models;

namespace BLL.Services;

public class ResourcesService : IResourcesService
{
    private readonly IResourcesAccessor _resourcesAccessor;
    private readonly IOpenAIClientService _openAIClientService;
    private readonly ILanguageService _languageService;

    public ResourcesService(IResourcesAccessor resourcesAccessor, IOpenAIClientService openAIClientService, ILanguageService languageService)
    {
        _resourcesAccessor = resourcesAccessor;
        _openAIClientService = openAIClientService;
        _languageService = languageService;
    }

    /// <inheritdoc cref="IResourcesService.GetRelatedResourcesAsync(string)"/>
    public async Task<List<ResourcesModelDto>> GetRelatedResourcesAsync(string textUserInput)
    {
        var cosineDistance = EmbeddingHelper.GetCosineDistanceByLanguage(_languageService.CurrentLanguage);
        var userEmbeddedRequest = await _openAIClientService.EmbedUserRequestAsync(textUserInput);
        var relatedResourcesList = await _resourcesAccessor.GetRelatedResources(userEmbeddedRequest, cosineDistance);

        var response = MapResourcesToDtos(relatedResourcesList);

        return response;
    }

    /// <summary>
    /// Converts a collection of ResourcesModel objects to a list of ResourcesModelDto objects.
    /// </summary>
    /// <param name="resources">The collection of ResourcesModel objects to be converted.</param>
    /// <returns>A list of ResourcesModelDto objects containing the mapped data.</returns>
    private static List<ResourcesModelDto> MapResourcesToDtos(IEnumerable<ResourcesModel> resources)
    {
        return resources.Select(resource => new ResourcesModelDto
        {
            Title = resource.Title,
            UrlPath = resource.UrlPath
        }).ToList();
    }
}