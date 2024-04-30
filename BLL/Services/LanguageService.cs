using BLL.Interfaces;
using System.Reflection;
using System.Resources;
using DLL.Enums;
using LanguageDetection;

namespace BLL.Services;

public class LanguageService : ILanguageService
{
    private LanguageEnum _currentLanguage = LanguageEnum.English;
    private readonly ResourceManager _resourceManager;
    private readonly LanguageDetector _languageDetector;

    /// <summary>
    /// Initializes a new instance of the LanguageService class.
    /// </summary>
    /// <param name="baseName">The base name of the resource file containing language strings.</param>
    /// <param name="assembly">The assembly containing the resource file.</param>
    public LanguageService(string baseName, Assembly assembly)
    {
        _resourceManager = new ResourceManager(baseName, assembly);
        _languageDetector = new LanguageDetector();
        _languageDetector.AddLanguages("ara", "eng");
    }

    ///<inheritdoc cref="ILanguageService.CurrentLanguage"/>
    public LanguageEnum CurrentLanguage => _currentLanguage;

    ///<inheritdoc cref="ILanguageService.SetLanguage(LanguageEnum)"/>
    public void SetLanguage(LanguageEnum language)
    {
        _currentLanguage = language;
    }

    ///<inheritdoc cref="ILanguageService.GetGreeting"/>
    public string? GetGreeting()
    {
        return _currentLanguage switch
        {
            LanguageEnum.English => _resourceManager.GetString("GREETING_EN"),
            LanguageEnum.Arabic => _resourceManager.GetString("GREETING_AR"),
            _ => throw new ArgumentOutOfRangeException(nameof(_currentLanguage), _currentLanguage, null)
        };
    }

    ///<inheritdoc cref="ILanguageService.GetWarning(LanguageEnum)"/>
    public string? GetWarning(LanguageEnum language)
    {
        return language switch
        {
            LanguageEnum.English => _resourceManager.GetString("WARNING_MESSAGE_EN"),
            LanguageEnum.Arabic => _resourceManager.GetString("WARNING_MESSAGE_AR"),
            _ => throw new ArgumentOutOfRangeException(nameof(_currentLanguage), _currentLanguage, null)
        };
    }

    ///<inheritdoc cref="ILanguageService.DetectLanguage(string)"/>
    public string DetectLanguage(string text)
    {
        return _languageDetector.Detect(text); 
    }
}