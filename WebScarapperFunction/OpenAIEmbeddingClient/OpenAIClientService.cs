using Azure;
using Azure.AI.OpenAI;

namespace WebScrapperFunction.OpenAIEmbeddingClient;

public class OpenAIClientService : IOpenAIClientService
{
    private readonly string _endpoint = "https://aichatbot-service-001.openai.azure.com/";
    private readonly string _key = "2c4d3ad2b7564de8af497cd363e2fb74";
    private readonly string _deploymentName = "text-embedding-ada-002";

    public async Task<List<EmbeddingItem>> ProcessTitles(IEnumerable<string> titlesToEmbed)
    {
        var client = new OpenAIClient(new Uri(_endpoint), new AzureKeyCredential(_key));

        var tasks = titlesToEmbed.Select(async message =>
        {
            var chatEmbeddingOptions = new EmbeddingsOptions
            {
                Input = { message }
            };

            try
            {
                var response = await client.GetEmbeddingsAsync(_deploymentName, chatEmbeddingOptions);
                return response.Value.Data;
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error processing user message '{message}': {ex.Message}");
                return Array.Empty<EmbeddingItem>();
            }
        });

        var allEmbeddings = await Task.WhenAll(tasks);
        return allEmbeddings.SelectMany(embeddings => embeddings).ToList();
    }
}
