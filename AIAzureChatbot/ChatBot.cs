using DLL.Accessors;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLL.Interfaces;
using DLL.Enums;
using DLL.Models;

namespace AIAzureChatBot;

public class ChatBot : ActivityHandler
{
    private readonly BotStateAccessor _stateAccessor;
    private readonly ILanguageService _languageService;
    private readonly ICommunicationService _communicationService;

    public ChatBot(ILanguageService languageService, BotStateAccessor stateAccessor, ICommunicationService communicationService)
    {
        _stateAccessor = stateAccessor;
        _communicationService = communicationService;
        _languageService = languageService;
    }
    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        var conversationData = await _stateAccessor.ConversationDataAccessor.GetAsync(turnContext, () => new ConversationData(), cancellationToken);
        if (conversationData.IsWelcomeMessagePerformed)
        {
            var response = await _communicationService.GenerateResponseMessageAsync(turnContext.Activity.Text);
            await turnContext.SendActivityAsync(MessageFactory.Text(response, response), cancellationToken);
        }
        else
        {
            if (turnContext.Activity != null && !conversationData.IsWelcomeMessagePerformed)
            {
                var language = turnContext.Activity.Text;
                _languageService.SetLanguage(Enum.TryParse<LanguageEnum>(language, out var parsedLanguage)
                    ? parsedLanguage
                    : LanguageEnum.English);
            }

            var response = _languageService.GetGreeting();
            conversationData.IsWelcomeMessagePerformed = true;

            await _stateAccessor.ConversationDataAccessor.SetAsync(turnContext, conversationData, cancellationToken);
            await _stateAccessor.ConversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);

            await turnContext.SendActivityAsync(MessageFactory.Text(response, response), cancellationToken);
        }
    }
    protected override async Task OnMembersAddedAsync(
        IList<ChannelAccount> membersAdded,
        ITurnContext<IConversationUpdateActivity> turnContext,
        CancellationToken cancellationToken)
    {
        foreach (var member in membersAdded)
        {
            if (member.Id == turnContext.Activity.Recipient.Id)
                continue;

            var activities = new IActivity[]
            {
                MessageFactory.Text(_languageService.GetWarning(LanguageEnum.English)),
                MessageFactory.Text(_languageService.GetWarning(LanguageEnum.Arabic))
            };

            await turnContext.SendActivitiesAsync(activities, cancellationToken);
            await SendSuggestedActionsAsync(turnContext, cancellationToken);
        }
    }

    private static async Task SendSuggestedActionsAsync(ITurnContext turnContext, CancellationToken cancellationToken)
    {
        var reply = MessageFactory.Text("Choose language: ");

        reply.SuggestedActions = new SuggestedActions
        {
            Actions = new List<CardAction>
            {
                new() { Title = "English", Type = ActionTypes.ImBack, Value = LanguageEnum.English.ToString() },
                new() { Title = "Arabic", Type = ActionTypes.ImBack, Value = LanguageEnum.Arabic.ToString() }
            },
        };
        await turnContext.SendActivityAsync(reply, cancellationToken);
    }
}