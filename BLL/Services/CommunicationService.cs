using BLL.Dtos;
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
    public async Task<ResponseActivity> GenerateResponseMessageAsync(string userInputMessage)
    {
        var gptResponseTask = _openAIClientService.GenerateGptResponseAsync(userInputMessage);
        var relatedResourcesTask = _resourceService.GetRelatedResourcesAsync(userInputMessage);

        await Task.WhenAll(relatedResourcesTask, gptResponseTask);

        var relatedResources = await relatedResourcesTask;
        var gptResponse = await gptResponseTask;

        var gptResponseMessage = CitationsHelper.RemoveDocN(gptResponse.Response);
        var resourcesResponse = BotMarkdownHelper.GetResourceLinksMarkdown(relatedResources, shouldHaveHr: false);

        var response = string.Concat(gptResponseMessage, resourcesResponse);
        
        return new ResponseActivity{Response = response, SuggestedIntents = gptResponse.Intents!};
    }
}