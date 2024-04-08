using AIAzureChatbot.Enums;

namespace BLL.Interfaces;

public interface ILanguageService
{
    LanguageEnum CurrentLanguage { get; }
    void SetLanguage(LanguageEnum language);
    string? GetGreeting();
    string? GetWarning(LanguageEnum language);
}