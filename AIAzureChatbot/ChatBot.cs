using System;
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

    private const string ArabicIsoCode = "ara";
    private const string ArabicLocaleCode = ActivityHelper.ArabicLocaleCode;

    private readonly BotStateAccessor _stateAccessor;
    private readonly ILanguageService _languageService;
    private readonly ICommunicationService _communicationService;

    public ChatBot(
        ILanguageService languageService, 
        BotStateAccessor stateAccessor, 
        ICommunicationService communicationService
        )
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

            var detectedLanguage = _languageService.DetectLanguage(response.Response);
            
            if(detectedLanguage.Equals(ArabicIsoCode, StringComparison.InvariantCulture))
                responseActivity.ChannelData = new { locale = ArabicLocaleCode };

            var activitiesList = response.SuggestedIntents.Select(suggestion => new CardAction { Title = suggestion, Type = ActionTypes.ImBack, Value = suggestion }).ToList();

            ActivityHelper.AttachSuggestedActions(responseActivity, activitiesList);

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

            var actionMessage = MessageFactory.Text(response, response);

            if (_languageService.CurrentLanguage == LanguageEnum.Arabic)
                actionMessage.ChannelData = new { locale = ArabicLocaleCode };

            await turnContext.SendActivityAsync(actionMessage, cancellationToken);
        }
    }
    protected override async Task OnMembersAddedAsync(
        IList<ChannelAccount> membersAdded,
        ITurnContext<IConversationUpdateActivity> turnContext,
        CancellationToken cancellationToken)
    {
        foreach (var member in membersAdded)
        {
            if (turnContext.Activity?.Recipient is not null && member.Id == turnContext.Activity.Recipient.Id)
                continue;

            IActivity[] activities = {
                ActivityHelper.CreateActivity(_languageService.GetWarning(LanguageEnum.English)!, LanguageEnum.English),
                ActivityHelper.CreateActivity(_languageService.GetWarning(LanguageEnum.Arabic)!, LanguageEnum.Arabic)
            };

            var lastActivity = activities[^1];

            ActivityHelper.AttachSuggestedActions((Activity)lastActivity, new List<CardAction>
            {
                new() { Title = EnglishTitle, Type = ActionTypes.ImBack, Value = EnglishTitle },
                new() { Title = ArabicTitle, Type = ActionTypes.ImBack, Value = ArabicTitle }
            });

            await turnContext.SendActivitiesAsync(activities, cancellationToken);
        }
    }
}