using AIAzureChatbot.Enums;

namespace BLL.Interfaces;

public interface ICommunicationService
{
    /// <summary>
    /// Generates a comprehensive response to a user's input message, combining a GPT-3 generated response with relevant resources.
    /// </summary>
    /// <param name="userInputMessage">The user's message to respond to.</param>
    /// <param name="currentLanguage">The language to use for the response (e.g., "en" for English).</param>
    /// <returns>A string containing the combined response, including both text and resource links.</returns>
    Task<string> GenerateResponseMessageAsync(string userInputMessage, LanguageEnum currentLanguage);
}