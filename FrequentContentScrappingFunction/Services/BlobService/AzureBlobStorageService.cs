using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;

namespace FrequentContentScrappingFunction.Services.BlobService;

public class AzureBlobStorageService : IAzureBlobStorageService
{
    // Azure Blob Storage Environment Variables
    private readonly string _blobStorageContainerName = EnvironmentVariables.BlobStorageContainerName!;

    private readonly BlobContainerClient _containerClient;

    public AzureBlobStorageService(BlobServiceClient blobServiceClient)
    {
        _containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageContainerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task UploadBlobAsync(string blobName, Stream stream)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(stream, true);
    }

    public async Task AppendToBlobAsync(string blobName, MemoryStream logEntryStream)
    {
        var appendBlobClient = _containerClient.GetAppendBlobClient(blobName);

        // Ensure the Append Blob exists
        await appendBlobClient.CreateIfNotExistsAsync();

        // Define the maximum block size for the append blob
        var maxBlockSize = appendBlobClient.AppendBlobMaxAppendBlockBytes;
        var bytesLeft = logEntryStream.Length;

        // Reset the stream position to the beginning
        logEntryStream.Position = 0;

        // Buffer to hold the data read from the MemoryStream
        var buffer = new byte[maxBlockSize];

        while (bytesLeft > 0)
        {
            // Determine the size of the current block
            var blockSize = (int)Math.Min(bytesLeft, maxBlockSize);

            // Read the data from the MemoryStream into the buffer
            var bytesRead = await logEntryStream.ReadAsync(buffer.AsMemory(0, blockSize));

            // Create a new MemoryStream containing the current block of data
            await using (var memoryStream = new MemoryStream(buffer, 0, bytesRead))
            {
                // Append the block to the append blob
                await appendBlobClient.AppendBlockAsync(memoryStream);
            }

            // Decrease the number of bytes left to read
            bytesLeft -= bytesRead;
        }
    }

    //public async Task<Stream> DownloadBlobAsync(string blobName)
    //{
    //    var blobServiceClient = new BlobServiceClient(_blobStorageConnectionString);
    //    var blobContainerClient = blobServiceClient.GetBlobContainerClient(_blobStorageContainerName);
    //    var blobClient = blobContainerClient.GetBlobClient(blobName);

    //    var blobDownloadInfo = await blobClient.DownloadAsync();
    //    return blobDownloadInfo.Value.Content;
    //}

    //public async Task DeleteBlobAsync(string blobName)
    //{
    //    var blobServiceClient = new BlobServiceClient(_blobStorageConnectionString);
    //    var blobContainerClient = blobServiceClient.GetBlobContainerClient(_blobStorageContainerName);
    //    var blobClient = blobContainerClient.GetBlobClient(blobName);

    //    await blobClient.DeleteIfExistsAsync();
    //}
}