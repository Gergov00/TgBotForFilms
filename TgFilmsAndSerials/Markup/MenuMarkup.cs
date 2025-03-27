using Telegram.Bot.Types.ReplyMarkups;

namespace TgFilmsAndSerials.Markup;

public static class MenuMarkup
{
    public static InlineKeyboardMarkup GetMainMenuMarkup() =>
        new(new[]
        {
            new [] { InlineKeyboardButton.WithCallbackData("🎲 Случайный фильм", "/random") },
            new [] { InlineKeyboardButton.WithCallbackData("⭐ Избранное", "/favourites") },
            new [] { InlineKeyboardButton.WithCallbackData("🔍 Фильтр", "/settings") },
        });
}
