using AIAzureChatbot.Enums;

namespace AIAzureChatbot.Interfaces
{
    public interface ILanguageService
    {
        LanguageEnum CurrentLanguage { get; }
        void SetLanguage(LanguageEnum language);
        string GetGreeting();
        string GetWarning(LanguageEnum language);
    }
}
