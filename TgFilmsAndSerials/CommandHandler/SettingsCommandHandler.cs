using Data.Entities;
using Data.Sorage;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgFilmsAndSerials;



namespace TgFilmsAndSerials.CommandHandler;

public class SettingsCommandHandler : ICommandHandler
{
    private readonly UserFilterStorage _filterStorage;
    private readonly IServiceProvider _serviceProvider;
    public SettingsCommandHandler(UserFilterStorage filterStorage, IServiceProvider serviceProvider)
    {
        _filterStorage = filterStorage;
        _serviceProvider = serviceProvider;
    }

    public string Command => "/settings";

    public async Task HandleAsync(string? args, TelegramBotClient bot, CallbackQuery? callbackQuery)
    {
        _filterStorage.Filters[callbackQuery.From.Id] = new UserFilter();

        await bot.EditMessageTextAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, "Выберите жанры:",
            replyMarkup: new InlineKeyboardMarkup(new[]
            {
                new [] { InlineKeyboardButton.WithCallbackData("🎭 Драма", "genre:драма") },
                new [] { InlineKeyboardButton.WithCallbackData("😱 Ужасы", "genre:ужасы") },
                new [] { InlineKeyboardButton.WithCallbackData("😂 Комедия", "genre:комедия") },
                new [] { InlineKeyboardButton.WithCallbackData("👽 Фантастика", "genre:фантастика") },
                new [] { InlineKeyboardButton.WithCallbackData("🕵️‍♂️ Детектив", "genre:детектив") },
                new [] { InlineKeyboardButton.WithCallbackData("🗡️ Боевик", "genre:боевик") },
                new [] { InlineKeyboardButton.WithCallbackData("🌄 Приключения", "genre:приключения") },
                new [] { InlineKeyboardButton.WithCallbackData("❤️ Мелодрама", "genre:мелодрама") },
                new [] { InlineKeyboardButton.WithCallbackData("📜 Исторический", "genre:исторический") },
                new [] { InlineKeyboardButton.WithCallbackData("🧙‍♂️ Фэнтези", "genre:фэнтези") },
                new [] { InlineKeyboardButton.WithCallbackData("➡ Далее", "next:countries") },
            }));
    }

    public async Task HandleSelectionAsync(TelegramBotClient bot, CallbackQuery query)
    {
        var userId = query.From.Id;
        if (!_filterStorage.Filters.TryGetValue(userId, out var filter))
        {
            filter = new UserFilter();
            _filterStorage.Filters[userId] = filter;
        }

        if (query.Data.StartsWith("genre:"))
        {
            var genre = query.Data["genre:".Length..];
            if (!filter.Genres.Contains(genre))
                filter.Genres.Add(genre);
            await bot.AnswerCallbackQueryAsync(query.Id, $"Жанр добавлен: {genre}");
        }
        else if (query.Data.StartsWith("country:"))
        {
            var country = query.Data["country:".Length..];
            if (!filter.Countries.Contains(country))
                filter.Countries.Add(country);
            await bot.AnswerCallbackQueryAsync(query.Id, $"Страна добавлена: {country}");
        }
        else if (query.Data == "next:year")
        {
            await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, "Выберите начальный год:",
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new[] { InlineKeyboardButton.WithCallbackData("2024", "year_from:2024") },
                    new[] { InlineKeyboardButton.WithCallbackData("2020", "year_from:2020") },
                    new[] { InlineKeyboardButton.WithCallbackData("2015", "year_from:2015") },
                    new[] { InlineKeyboardButton.WithCallbackData("2010", "year_from:2010") },
                    new[] { InlineKeyboardButton.WithCallbackData("2000", "year_from:2000") },
                }));
        }
        else if (query.Data.StartsWith("year_from:"))
        {
            if (int.TryParse(query.Data["year_from:".Length..], out var yearFrom))
            {
                filter.YearFrom = yearFrom;

                await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, $"Год от: {yearFrom}\nТеперь выберите конечный год:",
                    replyMarkup: new InlineKeyboardMarkup(new[]
                    {
                        new[] { InlineKeyboardButton.WithCallbackData("2024", "year_to:2024") },
                        new[] { InlineKeyboardButton.WithCallbackData("2020", "year_to:2020") },
                        new[] { InlineKeyboardButton.WithCallbackData("2015", "year_to:2015") },
                        new[] { InlineKeyboardButton.WithCallbackData("2010", "year_to:2010") },
                        new[] { InlineKeyboardButton.WithCallbackData("2000", "year_to:2000") },
                    }));
            }
        }
        else if (query.Data.StartsWith("year_to:"))
        {
            if (int.TryParse(query.Data["year_to:".Length..], out var yearTo))
            {
                filter.YearTo = yearTo;

                await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId,
                    $"Выбран период: {filter.YearFrom}-{filter.YearTo}",
                    replyMarkup: new InlineKeyboardMarkup(new[]
                    {
                        new[] { InlineKeyboardButton.WithCallbackData("🔍 Применить фильтр", "apply") },
                        new[] { InlineKeyboardButton.WithCallbackData("❌ Сбросить фильтр", "reset") }
                    }));
            }
        }

        else if (query.Data == "next:countries")
        {
            await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, "Выберите страны:",
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new [] { InlineKeyboardButton.WithCallbackData("🇺🇸 США", "country:США") },
                    new [] { InlineKeyboardButton.WithCallbackData("🇷🇺 Россия", "country:Россия") },
                    new [] { InlineKeyboardButton.WithCallbackData("🇬🇧 Великобритания", "country:Великобритания") },
                    new [] { InlineKeyboardButton.WithCallbackData("🇫🇷 Франция", "country:Франция") },
                    new [] { InlineKeyboardButton.WithCallbackData("🇰🇷 Южная Корея", "country:Южная Корея") },
                    new [] { InlineKeyboardButton.WithCallbackData("🇯🇵 Япония", "country:Япония") },
                    new [] { InlineKeyboardButton.WithCallbackData("➡ Далее", "next:year") },
                }));

        }
        else if (query.Data == "next:year")
        {
            await bot.EditMessageTextAsync(query.Message.Chat.Id, query.Message.MessageId, "Выберите год:",
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new [] { InlineKeyboardButton.WithCallbackData("🇺🇸 США", "country:США") },
                    new [] { InlineKeyboardButton.WithCallbackData("🇷🇺 Россия", "country:Россия") },
                    new [] { InlineKeyboardButton.WithCallbackData("🇬🇧 Великобритания", "country:Великобритания") },
                    new [] { InlineKeyboardButton.WithCallbackData("🇫🇷 Франция", "country:Франция") },
                    new [] { InlineKeyboardButton.WithCallbackData("🇰🇷 Южная Корея", "country:Южная Корея") },
                    new [] { InlineKeyboardButton.WithCallbackData("🇯🇵 Япония", "country:Япония") },
                    new [] { InlineKeyboardButton.WithCallbackData("➡ Далее", "next:year") },
                }));
        }
        else if (query.Data == "apply")
        {
            //var args = $"{string.Join(",", filter.Genres)}|{string.Join(",", filter.Countries)}|{filter.Year}";

            var dispatcher = _serviceProvider.GetRequiredService<CommandDispatcher>();
            await dispatcher.DispatchAsync("/random", bot, query, null);
        }
        else if (query.Data == "reset")
        {
            _filterStorage.Filters.Remove(userId);
            await bot.AnswerCallbackQueryAsync(query.Id, "Фильтр сброшен.");
        }
    }
}

