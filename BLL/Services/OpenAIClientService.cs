using AIAzureChatbot.Enums;
using AIAzureChatBot.OpenAIClientService;
using Azure;
using Azure.AI.OpenAI;
using Common;
using Common.Helpers;
using Pgvector;

namespace BLL.Services;

public class OpenAIClientService : IOpenAIClientService
{
    private readonly string _azureOpenAiGptEndpoint = EnvironmentVariables.AzureOpenAiGptEndpoint!;
    private readonly string _azureOpenAIGptKey = EnvironmentVariables.AzureOpenAIGptKey!;
    private readonly string _deploymentGptName = EnvironmentVariables.DeploymentGptName!;
    private readonly string _searchEndpoint = "https://kmchatbot-search-zuk2xzn7vbusc.search.windows.net";
    private readonly string _searchKey = EnvironmentVariables.SearchKey!;
    private readonly string _searchIndex = EnvironmentVariables.SearchIndex!;

    private readonly string _endpoint = EnvironmentVariables.EmbeddingEndpoint!;
    private readonly string _key = EnvironmentVariables.EmbeddingKey!;
    private readonly string _deploymentName = EnvironmentVariables.EmbeddingDeploymentName!;

    public async Task<string> ProcessUserMessageGpt(string userMessage, LanguageEnum language)
    {
        var client = new OpenAIClient(new Uri(_azureOpenAiGptEndpoint), new AzureKeyCredential(_azureOpenAIGptKey));

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
            DeploymentName = _deploymentGptName,
            MaxTokens = 800,
            Temperature = 0.1f,
        };

        try
        {
            var response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
            var responseMessage = response.Value.Choices[0].Message;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return CitationsHelper.ReplaceDocWithSuperscript("sdasdas");
    }

    public async Task<Vector> EmbedUserRequest(string request)
    {
        var client = new OpenAIClient(new Uri(_endpoint), new AzureKeyCredential(_key));

        var chatEmbeddingOptions = new EmbeddingsOptions
        {
            Input = { request },
            DeploymentName = _deploymentName
        };

        var embeddingResponse = await client.GetEmbeddingsAsync(chatEmbeddingOptions);

        var response = embeddingResponse.Value.Data[0].Embedding.ToArray();

        return new Vector(response);
    }
}
