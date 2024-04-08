using AIAzureChatBot.OpenAIClientService;
using BLL.Dtos;
using BLL.Interfaces;
using DLL.Interfaces;
using DLL.Models;

namespace BLL.Services;

public class ResourcesService : IResourcesService
{
    private readonly IResourcesAccessor _resourcesAccessor;
    private readonly IOpenAIClientService _openAIClientService;

    public ResourcesService(IResourcesAccessor resourcesAccessor, IOpenAIClientService openAIClientService)
    {
        _resourcesAccessor = resourcesAccessor;
        _openAIClientService = openAIClientService;
    }

    public async Task<List<ResourcesModelDto>> GetRelatedResourcesAsync(string textUserInput)
    {
        var userEmbeddedRequest = await _openAIClientService.EmbedUserRequest(textUserInput);
        var relatedResourcesList = await _resourcesAccessor.GetRelatedResources(userEmbeddedRequest);

        var response = MapResourcesToDtos(relatedResourcesList);

        return response;
    }

    private static List<ResourcesModelDto> MapResourcesToDtos(IEnumerable<ResourcesModel> resources)
    {
        return resources.Select(resource => new ResourcesModelDto
        {
            Title = resource.Title,
            UrlPath = resource.UrlPath
        }).ToList();
    }
}