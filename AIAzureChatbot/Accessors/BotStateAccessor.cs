using Microsoft.Bot.Builder;
using System;
using AIAzureChatbot.Models;

namespace AIAzureChatbot.Accessors
{
    public class BotStateAccessor
    {
        public ConversationState ConversationState { get; }

        public BotStateAccessor(ConversationState conversationState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
        }
        public IStatePropertyAccessor<ConversationData> ConversationDataAccessor { get; set; }

        public static string ConversationDataName { get; } = "ConversationData";
    }
}
