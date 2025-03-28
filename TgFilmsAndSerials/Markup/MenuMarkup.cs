using Telegram.Bot.Types.ReplyMarkups;

namespace TgFilmsAndSerials.Markup;

public static class MenuMarkup
{
    public static InlineKeyboardMarkup GetMainMenuMarkup() =>
        new(new[]
        {
            new [] { InlineKeyboardButton.WithCallbackData("🎲 Случайный фильм", "/random"), 
                InlineKeyboardButton.WithCallbackData("🔍 Поиск по назвaнию", "/search") },
            new [] { InlineKeyboardButton.WithCallbackData("⭐ Избранное", "/favourites") },
            new [] { InlineKeyboardButton.WithCallbackData("🔍 Фильтр", "/settings"), InlineKeyboardButton.WithCallbackData("🔍 Посмотреть фильтр", "/filter") },
            new [] { InlineKeyboardButton.WithCallbackData("❌ Сбросить фильтр", "/clearfilter") },

        });
}
