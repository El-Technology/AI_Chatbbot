using AIAzureChatbot.Enums;
using Pgvector;

namespace BLL.Interfaces;

public interface IOpenAIClientService
{
    /// <summary>
    /// Generates a response to a user message using OpenAI's GPT-3 language model, 
    /// potentially incorporating relevant information from Azure Cognitive Search.
    /// </summary>
    /// <param name="userMessage">The user's message to be responded to.</param>
    /// <param name="language">The language to use for the response from LanguageEnum.</param>
    /// <returns>The generated response message as a string.</returns>
    Task<string> GenerateGptResponseAsync(string userMessage, LanguageEnum language);

    /// <summary>
    /// Generates a vector embedding for a user's request using OpenAI's API.
    /// </summary>
    /// <param name="request">The user's request to be converted into an embedding.</param>
    /// <returns>A Vector object representing the numerical embedding of the request.</returns>
    Task<Vector> EmbedUserRequestAsync(string request);
}