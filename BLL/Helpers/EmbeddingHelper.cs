using DLL.Enums;

namespace BLL.Helpers;

public class EmbeddingHelper
{
    private const double EnCosineDistance = 0.175;
    private const double ArCosineDistance = 0.235;
    public static double GetCosineDistanceByLanguage(LanguageEnum language)
    {
        return language switch
        {
            LanguageEnum.English => EnCosineDistance,
            LanguageEnum.Arabic => ArCosineDistance,
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };
    }
}