using AIAzureChatbot.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

namespace AIAzureChatbot.Helpers;

public static class CitationsHelper
{
    public static string ReplaceDocWithSuperscript(string input)
    {
        const string pattern = @"\[doc(\d+)\]";

        var regex = new Regex(pattern);

        var result = regex.Replace(input, m => Superscript(m.Groups[1].Value));

        return result;
    }

    public static string Superscript(string number)
    {
        string[] superscripts = { "⁰", "¹", "²", "³", "⁴", "⁵", "⁶", "⁷", "⁸", "⁹" };

        var superscriptNumber = "";
        foreach (var digit in number)
        {
            var index = digit - '0';
            if (index >= 0 && index < superscripts.Length)
                superscriptNumber += superscripts[index] + " ";
            else
                superscriptNumber += digit;
        }

        return superscriptNumber;
    }

    public static string ConvertWithClickableLinks(string jsonString)
    {
        var rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
        
        var response = new StringBuilder();
       
        foreach (var citation in rootObject.Citations)
        {
            // Append the content with line breaks
            response.AppendLine(citation.Title + "-" + citation.Url);
            response.AppendLine();
        }

        return response.ToString();
    }
}