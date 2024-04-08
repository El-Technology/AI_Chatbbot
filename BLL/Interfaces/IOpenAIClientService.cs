using AIAzureChatbot.Enums;
using Pgvector;

namespace BLL.Interfaces;

public interface IOpenAIClientService
{
    Task<string> GenerateGptResponseAsync(string userMessage, LanguageEnum language);

    Task<Vector> EmbedUserRequest(string request);
}