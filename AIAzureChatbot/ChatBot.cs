using BLL.Helpers;
using BLL.Interfaces;
using DLL.Accessors;
using DLL.Enums;
using DLL.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AIAzureChatBot;

public class ChatBot : ActivityHandler
{
    private const string ArabicTitle =  "العربية";
    private const string EnglishTitle = "English";

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

        await turnContext.SendActivityAsync(new Activity { Type = ActivityTypes.Typing }, cancellationToken);

        if (conversationData.IsWelcomeMessagePerformed)
        {
            var response = await _communicationService.GenerateResponseMessageAsync(turnContext.Activity.Text);

            var responseActivity = MessageFactory.Text(response.Response, response.Response);

            var activitiesList = response.SuggestedIntents.Select(suggestion => new CardAction { Title = suggestion, Type = ActionTypes.ImBack, Value = suggestion }).ToList();

            AttachSuggestedActions(responseActivity, activitiesList);

            await turnContext.SendActivityAsync(responseActivity, cancellationToken);
        }
        else
        {
            if (turnContext.Activity != null && !conversationData.IsWelcomeMessagePerformed)
            {
                var language = turnContext.Activity.Text == ArabicTitle ? LanguageEnum.Arabic : LanguageEnum.English;
                _languageService.SetLanguage(language);
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

            IActivity[] activities = {
                MessageFactory.Text(_languageService.GetWarning(LanguageEnum.English)),
                MessageFactory.Text(_languageService.GetWarning(LanguageEnum.Arabic)!.ToRightAlignedArabic())
            };

            AttachSuggestedActions((Activity)activities[^1], new List<CardAction>
            {
                new() { Title = EnglishTitle, Type = ActionTypes.ImBack, Value = EnglishTitle },
                new() { Title = ArabicTitle, Type = ActionTypes.ImBack, Value = ArabicTitle }
            });

            await turnContext.SendActivitiesAsync(activities, cancellationToken);
        }
    }

    private static void AttachSuggestedActions(IMessageActivity activity, IList<CardAction> actionsToAttach)
    {
        activity.SuggestedActions = new SuggestedActions
        {
            Actions = actionsToAttach
        };
    }
}