using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgFilmsAndSerials.CommandHandler;

public class SearchCommandHandler : ICommandHandler
{
    public string Command => "/search";
    public async Task HandleAsync(string? args, TelegramBotClient bot, CallbackQuery? callbackQuery)

    {
        await bot.SendMessage(callbackQuery.Message.Chat, "Введите названия фильма");
    }
}