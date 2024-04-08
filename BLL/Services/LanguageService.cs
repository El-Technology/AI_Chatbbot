using System.Reflection;
using System.Resources;
using AIAzureChatbot.Enums;
using BLL.Interfaces;

namespace BLL.Services;

public class LanguageService : ILanguageService
{
    private LanguageEnum _currentLanguage = LanguageEnum.English;
    private readonly ResourceManager _resourceManager;

    public LanguageService(string baseName, Assembly assembly)
    {
        _resourceManager = new ResourceManager(baseName, assembly);
    }

    public LanguageEnum CurrentLanguage => _currentLanguage;

    public void SetLanguage(LanguageEnum language)
    {
        _currentLanguage = language;
    }

    public string? GetGreeting()
    {
        return _currentLanguage switch
        {
            LanguageEnum.English => _resourceManager.GetString("GREETING_EN"),
            LanguageEnum.Arabic => _resourceManager.GetString("GREETING_AR"),
            _ => throw new ArgumentOutOfRangeException(nameof(_currentLanguage), _currentLanguage, null)
        };
    }

    public string? GetWarning(LanguageEnum language)
    {
        return language switch
        {
            LanguageEnum.English => _resourceManager.GetString("WARNING_MESSAGE_EN"),
            LanguageEnum.Arabic => _resourceManager.GetString("WARNING_MESSAGE_AR"),
            _ => throw new ArgumentOutOfRangeException(nameof(_currentLanguage), _currentLanguage, null)
        };
    }
}