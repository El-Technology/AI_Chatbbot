using Azure.AI.OpenAI;

namespace WebScrapperFunction.OpenAIEmbeddingClient;

public interface IOpenAIClientService
{
    Task<List<EmbeddingItem>> ProcessTitles(IEnumerable<string> titlesToEmbed);
}