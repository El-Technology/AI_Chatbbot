using BLL.Dtos;

namespace BLL.Interfaces;

public interface ICommunicationService
{
    /// <summary>
    /// Generates a comprehensive response to a user's input message, combining a GPT-3 generated response with relevant resources.
    /// </summary>
    /// <param name="userInputMessage">The user's message to respond to.</param>
    /// <returns>An object containing the combined response, including both text and resource links as Response and intents to continue conversation as SuggestedIntents.</returns>
    Task<ResponseActivity> GenerateResponseMessageAsync(string userInputMessage);
}