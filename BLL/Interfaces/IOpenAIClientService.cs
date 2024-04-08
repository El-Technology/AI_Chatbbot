using AIAzureChatbot.Enums;
using System.Threading.Tasks;
using Pgvector;

namespace AIAzureChatBot.OpenAIClientService;

public interface IOpenAIClientService
{
    Task<string> ProcessUserMessageGpt(string userMessage, LanguageEnum language);

    Task<Vector> EmbedUserRequest(string request);
}