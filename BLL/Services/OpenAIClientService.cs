using Azure;
using Azure.AI.OpenAI;
using BLL.Dtos;
using BLL.Helpers;
using BLL.Interfaces;
using Common;
using DLL.Enums;
using Pgvector;

namespace BLL.Services;

public class OpenAIClientService : IOpenAIClientService
{
    private readonly ILanguageService _languageService;

    // OpenAI GPT-3.5 Turbo Environment Variables
    private readonly string _azureOpenAiGptEndpoint = EnvironmentVariables.AzureOpenAiGptEndpoint!;
    private readonly string _azureOpenAIGptKey = EnvironmentVariables.AzureOpenAIGptKey!;
    private readonly string _deploymentGptName = EnvironmentVariables.DeploymentGptName!;

    //Azure Cognitive Search Environment Variables
    private readonly string _searchEndpoint = EnvironmentVariables.SearchEndpoint!;
    private readonly string _searchKey = EnvironmentVariables.SearchKey!;
    private readonly string _searchIndex = EnvironmentVariables.SearchIndex!;

    //Azure ADA-02 Environment Variables
    private readonly string _endpoint = EnvironmentVariables.EmbeddingEndpoint!;
    private readonly string _key = EnvironmentVariables.EmbeddingKey!;
    private readonly string _deploymentAdaName = EnvironmentVariables.EmbeddingDeploymentName!;

    public OpenAIClientService(ILanguageService languageService)
    {
        _languageService = languageService;
    }

    ///<inheritdoc cref="IOpenAIClientService.GenerateGptResponseAsync(string)"/>>
    public async Task<GptResponse> GenerateGptResponseAsync(string userMessage)
    {
        var client = new OpenAIClient(new Uri(_azureOpenAiGptEndpoint), new AzureKeyCredential(_azureOpenAIGptKey));
        
        var chatCompletionsOptions = new ChatCompletionsOptions
        {
            Messages =
            {
                new ChatRequestUserMessage($"Please provide responses in {_languageService.CurrentLanguage} only. Make sure every response is in {_languageService.CurrentLanguage}."),
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
            DeploymentName = _deploymentGptName,
            MaxTokens = 800,
            Temperature = 0.1f,
        };

        var response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
        var resources = response.Value.Choices[0].Message.AzureExtensionsContext.Messages[0].Content;
        var responseMessage = response.Value.Choices[0].Message.Content;

        var intentList = new List<string>();
        
        if (_languageService.CurrentLanguage != LanguageEnum.English)
            return new GptResponse { Response = responseMessage, Intents = intentList };

        intentList = JsonHelper.ParseIntents(resources);

        return new GptResponse { Response = responseMessage, Intents = intentList };
    }

    ///<inheritdoc cref="IOpenAIClientService.EmbedUserRequestAsync(string)"/>>
    public async Task<Vector> EmbedUserRequestAsync(string request)
    {
        var client = new OpenAIClient(new Uri(_endpoint), new AzureKeyCredential(_key));

        var chatEmbeddingOptions = new EmbeddingsOptions
        {
            Input = { request },
            DeploymentName = _deploymentAdaName
        };

        var embeddingResponse = await client.GetEmbeddingsAsync(chatEmbeddingOptions);

        var response = embeddingResponse.Value.Data[0].Embedding.ToArray();

        return new Vector(response);
    }
}
