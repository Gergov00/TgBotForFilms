using Data.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgFilmsAndSerials.CommandHandler;

public class SettingsCommandHandler : ICommandHandler
{
    private static Dictionary<long, UserFilter> _filters = new();

    public string Command => "/settings";

    public async Task HandleAsync(TelegramBotClient bot, CallbackQuery callbackQuery, string args)
    {
        _filters[callbackQuery.From.Id] = new UserFilter();

        await bot.EditMessageTextAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, "Выберите жанры:",
            replyMarkup: new InlineKeyboardMarkup(new[]
            {
                new [] { InlineKeyboardButton.WithCallbackData("🎭 Драма", "genre:драма") },
                new [] { InlineKeyboardButton.WithCallbackData("😱 Ужасы", "genre:ужасы") },
                new [] { InlineKeyboardButton.WithCallbackData("➡ Далее", "next:countries") },
            }));
    }

    public static async Task HandleSelectionAsync(TelegramBotClient bot, CallbackQuery query, CommandDispatcher dispatcher)
    {
        var filter = _filters[query.From.Id];

        if (query.Data.StartsWith("genre:"))
        {
            filter.Genres.Add(query.Data.Split(':')[1]);
            await bot.AnswerCallbackQueryAsync(query.Id, "Жанр добавлен");
        }
        else if (query.Data.StartsWith("country:"))
        {
            filter.Countries.Add(query.Data.Split(':')[1]);
            await bot.AnswerCallbackQueryAsync(query.Id, "Страна добавлена");
        }
        else if (query.Data.StartsWith("year:"))
        {
            filter.Year = int.Parse(query.Data.Split(':')[1]);
            await bot.AnswerCallbackQueryAsync(query.Id, "Год выбран");
        }
        else if (query.Data == "next:countries")
        {
            await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, "Выберите страны:",
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new [] { InlineKeyboardButton.WithCallbackData("🇺🇸 США", "country:США") },
                    new [] { InlineKeyboardButton.WithCallbackData("➡ Далее", "next:year") },
                }));
        }
        else if (query.Data == "next:year")
        {
            await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, "Выберите год:",
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new [] { InlineKeyboardButton.WithCallbackData("2023", "year:2023") },
                    new [] { InlineKeyboardButton.WithCallbackData("🔍 Найти фильм", "apply") },
                }));
        }
        else if (query.Data == "apply")
        {
            var args = $"{string.Join(",", filter.Genres)}|{string.Join(",", filter.Countries)}|{filter.Year}";
            await dispatcher.DispatchAsync("/filter", args, bot, query);
        }
    }
}
