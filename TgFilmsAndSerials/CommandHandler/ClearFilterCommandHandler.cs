using Data.Entities;
using Data.Sorage;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgFilmsAndSerials.Markup;

namespace TgFilmsAndSerials.CommandHandler;


    public class ClearFilterCommandHandler : ICommandHandler
    {
        public string Command => "/clearfilter";

        private readonly UserFilterStorage _filterStorage;

        public ClearFilterCommandHandler(UserFilterStorage filterStorage)
        {
            _filterStorage = filterStorage;
        }

        public async Task HandleAsync(string? args, TelegramBotClient bot, CallbackQuery? callbackQuery)

        {
            _filterStorage.Filters.Remove(callbackQuery.From.Id);
            await bot.AnswerCallbackQueryAsync(callbackQuery.Id, "Фильтр сброшен!");

            await bot.EditMessageTextAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId,
                "Фильтр успешно сброшен.", replyMarkup: MenuMarkup.GetMainMenuMarkup());
        }
    }
