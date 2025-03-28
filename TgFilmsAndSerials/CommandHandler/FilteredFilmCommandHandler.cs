using Data.Entities;
using Data.Sorage;
using Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgFilmsAndSerials.CommandHandler;
using TgFilmsAndSerials.Markup;

public class FilteredFilmCommandHandler : ICommandHandler
{
    private readonly UserFilterStorage _filter;

    public FilteredFilmCommandHandler(UserFilterStorage filter)
    {
        _filter = filter;
    }

    public string Command => "/filter";

    public async Task HandleAsync(TelegramBotClient bot, CallbackQuery callbackQuery)
    {
        var text = "Нет фильтра";

        if (_filter.Filters.TryGetValue(callbackQuery.From.Id, out UserFilter filter))
        {



            var years = filter.YearFrom == filter.YearTo
                ? $"{filter.YearFrom}"
                : $"{filter.YearFrom} - {filter.YearTo}";

            text = $"<b>Жанры:</b> {string.Join(", ", filter.Genres.Select(g => g))}\n" +
                       $"<b>Страны:</b> {string.Join(", ", filter.Countries.Select(c => c))}\n" +
                       $"<b>Год:</b> {years}";

        }
        

        await bot.EditMessageTextAsync(
            parseMode: ParseMode.Html,
            chatId: callbackQuery.Message.Chat.Id, 
            messageId: callbackQuery.Message.MessageId, 
            text: text,
            replyMarkup: new InlineKeyboardMarkup(
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Меню", "/menu"),
                }
            ));
    }


}