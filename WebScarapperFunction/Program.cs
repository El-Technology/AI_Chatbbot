using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebScrapperFunction.Accessors;
using WebScrapperFunction.Common;
using WebScrapperFunction.OpenAIEmbeddingClient;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddScoped<IOpenAIClientService, OpenAIClientService>()
            .AddScoped<IResourcesModelAccessor, ResourcesModelAccessor>()
            .AddDbContext<AIChatbotDbContext>(options => options.UseNpgsql(EnvironmentVariables.ConnectionString));
    })
    .Build();

host.Run();
