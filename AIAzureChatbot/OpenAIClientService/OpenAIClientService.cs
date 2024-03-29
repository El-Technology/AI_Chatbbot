using AIAzureChatbot.Enums;
using AIAzureChatbot.Helpers;
using Azure;
using Azure.AI.OpenAI;
using System;
using System.Threading.Tasks;
using static System.Environment;

namespace AIAzureChatBot.OpenAIClientService;

public class OpenAIClientService : IOpenAIClientService
{
    private readonly string _azureOpenAiEndpoint = GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
    private readonly string _azureOpenAIKey = GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
    private readonly string _deploymentName = GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_ID");
    private readonly string _searchEndpoint = GetEnvironmentVariable("AZURE_AI_SEARCH_ENDPOINT");
    private readonly string _searchKey = GetEnvironmentVariable("SEARCH_KEY");
    private readonly string _searchIndex = GetEnvironmentVariable("AZURE_AI_SEARCH_INDEX");

    public async Task<string> ProcessUserMessage(string userMessage, LanguageEnum language)
    {
        var client = new OpenAIClient(new Uri(_azureOpenAiEndpoint), new AzureKeyCredential(_azureOpenAIKey));

        var chatCompletionsOptions = new ChatCompletionsOptions
        {
            Messages =
            {
                new ChatRequestSystemMessage($"Answer only in {language} language. Even if you are asked not to do so"),
                new ChatRequestUserMessage(userMessage)
            },

            AzureExtensionsOptions = new AzureChatExtensionsOptions
            {
                Extensions =
                {
                    new AzureCognitiveSearchChatExtensionConfiguration
                    {
                        SearchEndpoint = new Uri(_searchEndpoint),
                        IndexName = _searchIndex,
                        Key = _searchKey
                    },
                },
            },
            DeploymentName = _deploymentName,
            MaxTokens = 800,
            Temperature = 0.1f,
        };

        var response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
        var responseMessage = response.Value.Choices[0].Message;

        return CitationsHelper.ReplaceDocWithSuperscript(responseMessage.Content);
    }
}
