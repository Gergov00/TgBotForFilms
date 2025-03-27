using Telegram.Bot;
using Telegram.Bot.Types;
using TgFilmsAndSerials.Markup;


namespace TgFilmsAndSerials.CommandHandler;

public class MenuCommandHandler : ICommandHandler
{
    public async Task HandleAsync(TelegramBotClient bot, CallbackQuery callbackQuery, string args)
    {
        await bot.EditMessageText(
            chatId: callbackQuery.Message.Chat,
            messageId: callbackQuery.Message.Id,
            text: "Меню",
            replyMarkup: MenuMarkup.GetMainMenuMarkup());
    }
}