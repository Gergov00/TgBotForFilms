using System.Net.Mime;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgFilmsAndSerials;
using TgFilmsAndSerials.CommandHandler;
using TgFilmsAndSerials.Markup;


// replace YOUR_BOT_TOKEN below, or set your TOKEN in Project Properties > Debug > Launch profiles UI > Environment variables

var handlers = new Dictionary<string, ICommandHandler>
{
    { "/random", new RandomFilmCommandHandler() },
    {"/favourites", new FavoritesFilmCommandHandler()},
    {"/menu", new MenuCommandHandler()}
};

var dispetcher = new CommandDispatcher(handlers);

var token = Environment.GetEnvironmentVariable("TOKEN") ?? Token.token;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient(token, cancellationToken: cts.Token);

var me = await bot.GetMe();
await bot.DeleteWebhook();          // you may comment this line if you find it unnecessary
await bot.DropPendingUpdates();     // you may comment this line if you find it unnecessary

bot.OnError += OnError;
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;

Console.WriteLine($"@{me.Username} is running... Press Escape to terminate");
while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
cts.Cancel(); // stop the bot


async Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception);
    await Task.Delay(2000, cts.Token);
}

async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is not { } text)
        Console.WriteLine($"Received a message of type {msg.Type}");
    else if (text.StartsWith('/'))
    {
        var space = text.IndexOf(' ');
        if (space < 0) space = text.Length;
        var command = text[..space].ToLower();
        if (command.LastIndexOf('@') is > 0 and int at) 
            if (command[(at + 1)..].Equals(me.Username, StringComparison.OrdinalIgnoreCase))
                command = command[..at];
            else
                return; 
        await OnCommand(command, text[space..].TrimStart(), msg);
    }
    else
    {
        await bot.SendMessage(msg.Chat, "Введите /start, чтобы бот заработал");
    }
}


async Task OnCommand(string command, string args, Message msg)
{
    if (command == "/start")
    {
        await bot.SendMessage(msg.Chat, "Меню", replyMarkup: MenuMarkup.GetMainMenuMarkup());
    }
    else
    {
        await bot.SendMessage(msg.Chat, "Введите /start, чтобы бот заработал");
    }
}

async Task OnUpdate(Update update)
{
    switch (update)
    {
        case { CallbackQuery: { } callbackQuery }: await OnCallbackQuery(callbackQuery); break;
        default: Console.WriteLine($"Received unhandled update {update.Type}"); break;
    };
}

async Task OnCallbackQuery(CallbackQuery callbackQuery)
{
    await dispetcher.DispatchAsync(callbackQuery.Data, "123", bot, callbackQuery);
}

async Task OnPollAnswer(PollAnswer pollAnswer)
{
    if (pollAnswer.User != null)
        await bot.SendMessage(pollAnswer.User.Id, $"You voted for option(s) id [{string.Join(',', pollAnswer.OptionIds)}]");
}