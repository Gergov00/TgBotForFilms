using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgFilmsAndSerials.CommandHandler;

    
public class CommandDispatcher
{
    private readonly Dictionary<string, ICommandHandler> _handlers;

    public CommandDispatcher(IEnumerable<ICommandHandler> handlers)
    {
        _handlers = handlers.ToDictionary(h => h.Command, h => h, StringComparer.OrdinalIgnoreCase);
    }

    public async Task DispatchAsync(string command, TelegramBotClient bot, CallbackQuery? callbackQuery, string args)
    {
        if (_handlers.TryGetValue(command, out var handler))
        {
            await handler.HandleAsync(args, bot, callbackQuery);
        }
        else
        {
            Console.WriteLine($"Не зарегестрированая команда {command}");
        }
    }

    public async Task DispatchAsync(string command, TelegramBotClient bot, Message message, string args)
    {
        if (_handlers.TryGetValue(command, out var handler))
        {
            await handler.HandleAsync(args, bot, message);
        }
        else
        {
            Console.WriteLine($"Не зарегестрированая команда {command}");
        }
    }

}