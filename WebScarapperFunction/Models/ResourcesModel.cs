using Pgvector;

namespace WebScrapperFunction.Models;
public class ResourcesModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; } = string.Empty;
    public string? UrlPath { get; set; } = string.Empty;
    public Vector? Embedding { get; set; }
}