namespace FrequentContentScrappingFunction.Services.BlobService;

public interface IAzureBlobStorageService
{
    Task UploadBlobAsync(string blobName, Stream stream);

    Task AppendToBlobAsync(string blobName, MemoryStream logEntryStream);
}