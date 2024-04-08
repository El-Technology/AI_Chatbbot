using AIAzureChatbot.Enums;

namespace BLL.Interfaces;

public interface ICommunicationService
{
    Task<string> GenerateResponseMessageAsync(string userInputMessage, LanguageEnum currentLanguage);
}