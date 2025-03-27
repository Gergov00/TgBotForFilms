using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgFilmsAndSerials.CommandHandler;

public class RandomFilmCommandHandler: ICommandHandler
{
    public async Task HandleAsync(TelegramBotClient bot, CallbackQuery callbackQuery, string args)
    {
        await bot.EditMessageText(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            text: "Случайное кино",
            replyMarkup: new InlineKeyboardButton[][]
            {
                ["В избраное"],
                ["Дальше"],
                [InlineKeyboardButton.WithCallbackData("Меню", "/menu"), ]

            });
    }
}