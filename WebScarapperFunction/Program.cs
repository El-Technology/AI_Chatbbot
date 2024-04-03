using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebScrapperFunction.OpenAIEmbeddingClient;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // Register your services here
        services.AddScoped<IOpenAIClientService, OpenAIClientService>();
    })
    .Build();

host.Run();
