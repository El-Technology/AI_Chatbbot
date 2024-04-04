using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebScrapperFunction.Accessors;
using WebScrapperFunction.OpenAIEmbeddingClient;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString")!;
        
        services.AddScoped<IOpenAIClientService, OpenAIClientService>()
            .AddScoped<IResourcesModelAccessor, ResourcesModelAccessor>()
            .AddDbContext<AIChatbotDbContext>(options => options.UseNpgsql(connectionString));
    })
    .Build();

host.Run();
