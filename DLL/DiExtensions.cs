using AIAzureChatbot.Models;
using DLL.Accessors;
using DLL.Context;
using DLL.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DLL;

public static class DiExtensions
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, string connectionString)
    {
        IStorage storage = new MemoryStorage();
        var conversationState = new ConversationState(storage);

        return services
            .AddDbContext<AIChatbotDbContext>(c => c.UseNpgsql(connectionString))
            .AddSingleton(_ => new BotStateAccessor(conversationState)
            {
                ConversationDataAccessor = conversationState.CreateProperty<ConversationData>(BotStateAccessor.ConversationDataName),
            })
            .AddScoped<IResourcesAccessor, ResourcesAccessor>();
    }
}