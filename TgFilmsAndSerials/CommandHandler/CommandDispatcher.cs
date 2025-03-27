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

    public async Task DispatchAsync(string command, string args, TelegramBotClient bot, CallbackQuery callbackQuery)
    {
        if (_handlers.TryGetValue(command, out var handler))
        {
            await handler.HandleAsync(bot, callbackQuery, args);
        }
        else
        {
            Console.WriteLine($"Не зарегестрированая команда {command}");
        }
    }
}