using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgFilmsAndSerials.CommandHandler;


public interface ICommandHandler
{
    string Command { get; }

    Task HandleAsync(string? args, TelegramBotClient bot, CallbackQuery? callbackQuery)
    {
        throw new Exception("Обращение без реализации метода");

    }

    Task HandleAsync(string? args, TelegramBotClient bot, Message? message)
    {
        throw new Exception("Обращение без реализации метода");
    }
}