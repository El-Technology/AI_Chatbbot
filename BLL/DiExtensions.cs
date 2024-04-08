using System.Reflection;
using AIAzureChatBot.OpenAIClientService;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL;

public static class DiExtensions
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services, string baseName, Assembly assembly)
    {
        return services
            .AddSingleton<ILanguageService>(provider => new LanguageService(baseName, assembly))
            .AddScoped<IOpenAIClientService, OpenAIClientService>()
            .AddScoped<IResourcesService, ResourcesService>();
    }
}