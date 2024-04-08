using AIAzureChatbot.Enums;
using AIAzureChatBot.OpenAIClientService;
using BLL.Helpers;
using BLL.Interfaces;
using System.Text;

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

    public async Task<string> GenerateResponseMessageAsync(string userInputMessage, LanguageEnum currentLanguage)
    {
        var relatedResources = await _resourceService.GetRelatedResourcesAsync(userInputMessage);

        return BotMarkdownHelper.GetResourceLinksMarkdownAsync(relatedResources);
    }
}