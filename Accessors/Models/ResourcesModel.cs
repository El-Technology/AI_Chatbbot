namespace Accessors.Models;

public class ResourcesModel
{
    public string Title { get; set; } = string.Empty;
    public string UrlPath { get; set; } = string.Empty;
    public List<float> TitleEmbedding { get; set; } = new();
}