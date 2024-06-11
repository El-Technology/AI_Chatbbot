namespace FrequentContentScrappingFunction;

public class EnvironmentVariables
{
    public static readonly string? ConnectionString = Environment.GetEnvironmentVariable("ConnectionString")!;

    public static readonly string? BlobStorageConnectionString =
        Environment.GetEnvironmentVariable("BlobStorageConnectionString")!;

    public static readonly string? BlobStorageContainerName =
        Environment.GetEnvironmentVariable("BlobStorageContainerName")!;
}