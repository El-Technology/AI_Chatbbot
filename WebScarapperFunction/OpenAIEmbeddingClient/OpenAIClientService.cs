using Azure;
using Azure.AI.OpenAI;
using Pgvector;
using WebScrapperFunction.Accessors.Models;

namespace WebScrapperFunction.OpenAIEmbeddingClient;

public class OpenAIClientService : IOpenAIClientService
{
    private readonly string _endpoint = "https://aichatbot-service-001.openai.azure.com/";
    private readonly string _key = "2c4d3ad2b7564de8af497cd363e2fb74";
    private readonly string _deploymentName = "text-embedding-ada-002";

    public async Task EmbedResCollectionAsync(IEnumerable<ResourcesModel> resourcesToEmbed)
    {
        var client = new OpenAIClient(new Uri(_endpoint), new AzureKeyCredential(_key));

        var tasks = resourcesToEmbed.Select(async x =>
        {
            var chatEmbeddingOptions = new EmbeddingsOptions
            {
                Input = { x.Title },
                DeploymentName = _deploymentName
            };

            try
            {
                var response = await client.GetEmbeddingsAsync(chatEmbeddingOptions);
                x.Embedding = new Vector(response.Value.Data[0].Embedding.ToArray());
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error processing user message '{x.Title}': {ex.Message}");
            }
        });

        await Task.WhenAll(tasks);
    }
}
