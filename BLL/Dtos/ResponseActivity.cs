namespace BLL.Dtos;

public class ResponseActivity
{
    public string Response { get; set; } = string.Empty;
    public List<string> SuggestedIntents { get; set; } = new();
}