using AIAzureChatBot.OpenAIClientService;
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

    public async Task<List<ResourcesModel>> GetResources(string textUserInput)
    {
        var userEmbeddedRequest = await _openAIClientService.EmbedUserRequest(textUserInput);

        var response = await _resourcesAccessor.GetRelatedResources(userEmbeddedRequest);

        return response;
    }
}