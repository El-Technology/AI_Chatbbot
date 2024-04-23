namespace BLL.Dtos;
public class GptResponse
{
    public string Response { get; set; } = string.Empty;
    public List<string> Intents { get; set; } = new();
}