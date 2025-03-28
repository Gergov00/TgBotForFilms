using Telegram.Bot;
using Telegram.Bot.Types;
using TgFilmsAndSerials.Markup;


namespace TgFilmsAndSerials.CommandHandler;

public class MenuCommandHandler : ICommandHandler
{
    public string Command => "/menu";

    public async Task HandleAsync(string? args, TelegramBotClient bot, CallbackQuery? callbackQuery)

    {
        await bot.SendMessage(
            chatId: callbackQuery.Message.Chat,
            text: "Меню",
            replyMarkup: MenuMarkup.GetMainMenuMarkup());
    }
}