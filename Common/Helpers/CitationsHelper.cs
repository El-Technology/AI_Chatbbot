using System.Text.RegularExpressions;

namespace Common.Helpers;

public static class CitationsHelper
{
    public static string RemoveDocN(string input)
    {
        const string pattern = @"\[doc\d+\]";

        var regex = new Regex(pattern);
        var result = regex.Replace(input, string.Empty);

        return result;
    }
}