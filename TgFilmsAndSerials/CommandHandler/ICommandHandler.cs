using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgFilmsAndSerials.CommandHandler;


public interface ICommandHandler
{
    Task HandleAsync(TelegramBotClient bot, CallbackQuery callbackQuery, string args);
}