using AIAzureChatbot.Enums;
using System.Threading.Tasks;

namespace AIAzureChatBot.OpenAIClientService;

public interface IOpenAIClientService
{
    Task<string> ProcessUserMessage(string userMessage, LanguageEnum language);
}