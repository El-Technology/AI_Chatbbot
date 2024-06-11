using Azure.Storage.Blobs;
using FrequentContentScrappingFunction;
using FrequentContentScrappingFunction.Accessors;
using FrequentContentScrappingFunction.Services.AdditionalContentService;
using FrequentContentScrappingFunction.Services.BlobService;
using FrequentContentScrappingFunction.Services.BrowserService;
using FrequentContentScrappingFunction.Services.ContentExtractorService;
using FrequentContentScrappingFunction.Services.ScrappingService;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IAzureBlobStorageService, AzureBlobStorageService>(x =>
            new AzureBlobStorageService(new BlobServiceClient(EnvironmentVariables.BlobStorageConnectionString)));
        services.AddScoped<IScrappingService, ScrappingService>();
        services.AddScoped<IPageConfigurationsAccessor, PageConfigurationsAccessor>();
        services.AddScoped<IAdditionalContentService, AdditionalContentService>();
        services.AddScoped<IBrowserService, BrowserService>();
        services.AddScoped<IContentExtractor, ContentExtractor>();
        services.AddScoped<IScrappingService, ScrappingService>();

        services.AddDbContext<AIChatbotDbContext>(options => options.UseNpgsql(EnvironmentVariables.ConnectionString));
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();