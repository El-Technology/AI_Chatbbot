using DLL.Enums;

namespace BLL.Interfaces;

public interface ILanguageService
{
    /// <summary>
    /// Gets the currently selected language for the service.
    /// </summary>
    LanguageEnum CurrentLanguage { get; }

    /// <summary>
    /// Sets the language to be used for retrieving localized messages.
    /// </summary>
    /// <param name="language">The desired language to be used (e.g., LanguageEnum.English).</param>
    void SetLanguage(LanguageEnum language);

    /// <summary>
    /// Retrieves a greeting message based on the currently selected language.
    /// </summary>
    /// <returns>A string containing the greeting message in the selected language, or null if not found.</returns>
    string? GetGreeting();

    /// <summary>
    /// Retrieves a warning message based on the specified language.
    /// </summary>
    /// <param name="language">The language for which to retrieve the warning message (e.g., LanguageEnum.Arabic).</param>
    /// <returns>A string containing the warning message in the specified language, or null if not found.</returns>
    string? GetWarning(LanguageEnum language);
}