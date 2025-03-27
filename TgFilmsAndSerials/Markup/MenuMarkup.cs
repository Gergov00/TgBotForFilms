using Telegram.Bot.Types.ReplyMarkups;

namespace TgFilmsAndSerials.Markup;

public static class MenuMarkup
{
    public static InlineKeyboardMarkup GetMainMenuMarkup() =>
        new InlineKeyboardMarkup(new InlineKeyboardButton[][]
        {
            new [] { InlineKeyboardButton.WithCallbackData("Случайный фильм", "/random") },
            new [] { InlineKeyboardButton.WithCallbackData("Избраное", "/favourites") }
        });
}
