namespace WebScrapperFunction.Common;

public static class EnvironmentVariables
{
    public static readonly string? EmbeddingEndpoint = Environment.GetEnvironmentVariable("EmbeddingEndpoint")!;
    public static readonly string? EmbeddingKey = Environment.GetEnvironmentVariable("EmbeddingKey")!;
    public static readonly string? EmbeddingDeploymentName = Environment.GetEnvironmentVariable("EmbeddingDeploymentName")!;
    public static readonly string? SourceSiteUrl = Environment.GetEnvironmentVariable("SourceSiteUrl");
    public static readonly string? ConnectionString = Environment.GetEnvironmentVariable("ConnectionString")!;
}