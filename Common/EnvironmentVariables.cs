namespace Common;

public class EnvironmentVariables
{
    public static readonly string? EmbeddingEndpoint = Environment.GetEnvironmentVariable("EmbeddingEndpoint")!;
    public static readonly string? EmbeddingKey = Environment.GetEnvironmentVariable("EmbeddingKey")!;
    public static readonly string? EmbeddingDeploymentName = Environment.GetEnvironmentVariable("EmbeddingDeploymentName")!;

    public static readonly string? AzureOpenAiGptEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
    public static readonly string? AzureOpenAIGptKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
    public static readonly string? DeploymentGptName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_ID");
    public static readonly string? SearchEndpoint = Environment.GetEnvironmentVariable("AZURE_AI_SEARCH_ENDPOINT");
    public static readonly string? SearchKey = Environment.GetEnvironmentVariable("SEARCH_KEY");
    public static readonly string? SearchIndex = Environment.GetEnvironmentVariable("AZURE_AI_SEARCH_INDEX");

    public static readonly string? ConnectionString = Environment.GetEnvironmentVariable("ConnectionString")!;
}