using WebScrapperFunction.Accessors.Models;

namespace WebScrapperFunction.OpenAIEmbeddingClient;

public interface IOpenAIClientService
{
    Task EmbedResCollectionAsync(IEnumerable<ResourcesModel> resourcesToEmbed);
}