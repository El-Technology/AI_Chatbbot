using BLL.Helpers;
using BLL.Interfaces;
using Common.Helpers;

namespace BLL.Services;

public class CommunicationService : ICommunicationService
{
    private readonly IResourcesService _resourceService;
    private readonly IOpenAIClientService _openAIClientService;

    public CommunicationService(IResourcesService resourceService, IOpenAIClientService openAIClientService)
    {
        _resourceService = resourceService;
        _openAIClientService = openAIClientService;
    }

    ///<inheritdoc cref="ICommunicationService.GenerateResponseMessageAsync(string)"/>
    public async Task<string> GenerateResponseMessageAsync(string userInputMessage)
    {
        var textResponseTask = _openAIClientService.GenerateGptResponseAsync(userInputMessage);
        var relatedResourcesTask = _resourceService.GetRelatedResourcesAsync(userInputMessage);

        await Task.WhenAll(relatedResourcesTask, textResponseTask);

        var relatedResources = await relatedResourcesTask;
        var textResponse = await textResponseTask;

        var gptResponseMessage = CitationsHelper.RemoveDocN(textResponse);
        var resourcesResponse = BotMarkdownHelper.GetResourceLinksMarkdown(relatedResources, shouldHaveHr: false);

        var response = string.Concat(gptResponseMessage, resourcesResponse);

        return response;
    }
}