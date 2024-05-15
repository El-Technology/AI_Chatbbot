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

    /// <summary>
    /// Detects the language of the given text using the language detector.
    /// </summary>
    /// <param name="text">The text for which language detection is to be performed.</param>
    /// <returns>
    /// The ISO 639-3 language code representing the detected language of the input text.
    /// </returns>
    /// <remarks>
    /// This method internally utilizes a language detector object initialized with
    /// specific languages to be detected. The detected language is returned as an
    /// ISO 639-1 language code.
    /// </remarks>
    string DetectLanguage(string text);

    string? GetFallbackMessage();
}