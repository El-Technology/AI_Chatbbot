// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.22.0

using AIAzureChatbot.Enums;
using AIAzureChatBot.OpenAIClientService;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace AIAzureChatBot
{
    public class ChatBot : ActivityHandler
    {
        private readonly IOpenAIClientService _openAIClientService;
        private readonly ResourceManager _resourceManager;

        private string _warningMessageEn = string.Empty;
        private string _warningMessageAb = string.Empty;

        private static LanguageEnum _responseLanguage = LanguageEnum.English;
        
        private static bool _welcomeMessagePerformed = false;

        public ChatBot(IOpenAIClientService openAIClientService)
        {
            _openAIClientService = openAIClientService;
            _resourceManager = new ResourceManager("AIAzureChatbot.Resources.Resources", typeof(Program).Assembly);
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (_welcomeMessagePerformed)
            {
                var response = "Here we go!";
                //await _openAIClientService.ProcessUserMessage(turnContext.Activity.Text);
                await turnContext.SendActivityAsync(MessageFactory.Text(response, response), cancellationToken);
            }
            else
            {
                if (turnContext.Activity != null && !_welcomeMessagePerformed)
                {
                    var cardAction = turnContext.Activity;
                    if (cardAction is not null)
                    {
                        _responseLanguage = ProcessLanguage(cardAction.Text);
                    }
                }
                var response = LanguageWelcome(_responseLanguage);
                _welcomeMessagePerformed = true;
                await turnContext.SendActivityAsync(MessageFactory.Text(response, response), cancellationToken);
            }
        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id == turnContext.Activity.Recipient.Id)
                    continue;

                LoadWarningMessages();

                var activities = new IActivity[]
                {
                    MessageFactory.Text(_warningMessageEn),
                    MessageFactory.Text(_warningMessageAb)
                };

                await turnContext.SendActivitiesAsync(activities, cancellationToken);
                await SendSuggestedActionsAsync(turnContext, cancellationToken);
            }
        }

        private static LanguageEnum ProcessLanguage(string language)
        {
            var parsedLanguage = Enum.TryParse(language, out LanguageEnum parsedLang)
                ? parsedLang
                : LanguageEnum.Default;

            return parsedLanguage switch
            {
                LanguageEnum.English => _responseLanguage = LanguageEnum.English,
                LanguageEnum.Arabic => _responseLanguage = LanguageEnum.Arabic,
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            };
        }

        private string LanguageWelcome(LanguageEnum language)
        {
            return language switch
            {
                LanguageEnum.English => _resourceManager.GetString("GREETING_EN"),
                LanguageEnum.Arabic => _resourceManager.GetString("GREETING_AR"),
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            };
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

        private void LoadWarningMessages()
        {
            if (!string.IsNullOrEmpty(_warningMessageEn) && !string.IsNullOrEmpty(_warningMessageAb)) 
                return;
            _warningMessageEn = _resourceManager.GetString("WARNING_MESSAGE_EN");
            _warningMessageAb = _resourceManager.GetString("WARNING_MESSAGE_AR");
        }

    }
}
