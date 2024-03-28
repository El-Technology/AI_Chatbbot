using AIAzureChatbot.Enums;

namespace AIAzureChatbot.Interfaces
{
    public interface ILanguageService
    {
        LanguageEnum CurrentLanguage { get; }
        bool WelcomeMessagePerformed { get; set; }
        void SetLanguage(LanguageEnum language);
        string GetGreeting();
        string GetWarning(LanguageEnum language);
    }
}
