using AIAzureChatbot.Enums;
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

    ///<inheritdoc cref="ICommunicationService.GenerateResponseMessageAsync(string, LanguageEnum)"/>
    public async Task<string> GenerateResponseMessageAsync(string userInputMessage, LanguageEnum currentLanguage)
    {
        //var textResponseTask = _openAIClientService.GenerateGptResponseAsync(userInputMessage, currentLanguage);
        //var relatedResourcesTask = _resourceService.GetRelatedResourcesAsync(userInputMessage);

        //await Task.WhenAll(relatedResourcesTask, textResponseTask);

        //var relatedResources = await relatedResourcesTask;
        //var textResponse = await textResponseTask;

        //var gptResponseMessage = CitationsHelper.RemoveDocN(textResponse);
        //var resourcesResponse = BotMarkdownHelper.GetResourceLinksMarkdown(relatedResources, shouldHaveHr: true);

        //var response = gptResponseMessage + resourcesResponse;

        //return response;

        var relatedResources = await _resourceService.GetRelatedResourcesAsync(userInputMessage);

        var resourcesResponse1 = BotMarkdownHelper.GetResourceLinksMarkdown(relatedResources, shouldHaveHr: false);
        var resourcesResponse2 = BotMarkdownHelper.GetResourceLinksMarkdown(relatedResources, shouldHaveHr: true);

        return resourcesResponse1 + resourcesResponse2;
    }
}