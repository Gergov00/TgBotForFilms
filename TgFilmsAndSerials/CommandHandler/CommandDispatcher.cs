using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgFilmsAndSerials.CommandHandler;

    
public class CommandDispatcher
{
    private readonly Dictionary<string, ICommandHandler> commandHandlers;

    public CommandDispatcher(Dictionary<string, ICommandHandler> handlers)
    {
        commandHandlers = handlers;
    }

    public async Task DispatchAsync(string command, string args, TelegramBotClient bot, CallbackQuery callbackQuery)
    {
        if (commandHandlers.TryGetValue(command, out var handler))
        {
            await handler.HandleAsync(bot, callbackQuery, args);
        }
        else
        {
            Console.WriteLine($"Не зарегестрированая команда {command}");
        }
    }
}