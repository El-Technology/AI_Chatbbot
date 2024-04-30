using Newtonsoft.Json.Linq;

namespace BLL.Helpers;

public static class JsonHelper
{
    public static List<string> ParseIntents(string resources)
    {
        var jsonObject = JObject.Parse(resources);
        var intentArray = JArray.Parse(jsonObject["intent"]!.ToString());
        return intentArray.ToObject<List<string>>() ?? new List<string>();
    }
}