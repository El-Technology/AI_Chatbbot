using DLL.Enums;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace BLL.Helpers;

public class ActivityHelper
{
    public const string ArabicLocaleCode = "ar";
    public static void AttachSuggestedActions(IMessageActivity activity, IList<CardAction> actionsToAttach)
    {
        activity.SuggestedActions = new SuggestedActions
        {
            Actions = actionsToAttach
        };
    }

    public static Activity CreateActivity(string message, LanguageEnum language)
    {
        var messageActivity = MessageFactory.Text(message);

        if (language != LanguageEnum.Arabic) return messageActivity;

        messageActivity.ChannelData = new { locale = ArabicLocaleCode };
        messageActivity.Text.ToRightAlignedArabic();

        return messageActivity;
    }
}