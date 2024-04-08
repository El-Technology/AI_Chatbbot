using System.Text.RegularExpressions;

namespace Common.Helpers;

public static class CitationsHelper
{
    /// <summary>
    /// Removes any occurrences of citations in the format "[docN]" from a given string.
    /// </summary>
    /// <param name="input">The string potentially containing "[docN]" citations.</param>
    /// <returns>A new string with all "[docN]" citations removed.</returns>
    public static string RemoveDocN(string input)
    {
        const string pattern = @"\[doc\d+\]";

        var regex = new Regex(pattern);
        var result = regex.Replace(input, string.Empty);

        return result;
    }
}