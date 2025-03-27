using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgFilmsAndSerials;
using TgFilmsAndSerials.CommandHandler;
using TgFilmsAndSerials.Markup;
using Microsoft.Extensions.DependencyInjection;
using Services.Implementations;
using Services.Interfaces;

Console.WriteLine("Рабочая директория: " + Directory.GetCurrentDirectory());
Console.WriteLine("Путь к сборке: " + AppContext.BaseDirectory);


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

Console.WriteLine("MovieApi:ApiKey из конфигурации: " + configuration["MovieApi:ApiKey"]);


var services = new ServiceCollection();

services.AddSingleton<IConfiguration>(configuration);


services.AddHttpClient<IMovieService, MovieService>();
services.AddSingleton<IMovieService, MovieService>(); 
services.AddTransient<ICommandHandler, RandomFilmCommandHandler>();
services.AddTransient<ICommandHandler, FavoritesFilmCommandHandler>();
services.AddTransient<ICommandHandler, MenuCommandHandler>();
services.AddTransient<ICommandHandler, FilteredFilmCommandHandler>();
services.AddTransient<ICommandHandler, SettingsCommandHandler>();


services.AddSingleton<CommandDispatcher>();

var serviceProvider = services.BuildServiceProvider();

var dispatcher = serviceProvider.GetRequiredService<CommandDispatcher>();

var token = Environment.GetEnvironmentVariable("TOKEN") ?? Token.token;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient(token, cancellationToken: cts.Token);

var me = await bot.GetMe();
await bot.DeleteWebhook();
await bot.DropPendingUpdates();

bot.OnError += OnError;
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;

Console.WriteLine($"@{me.Username} is running... Press Escape to terminate");
while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
cts.Cancel(); // остановка бота

async Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception);
    await Task.Delay(2000, cts.Token);
}

async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text is not { } text)
    {
        Console.WriteLine($"Received a message of type {msg.Type}");
    }
    else if (text.StartsWith('/'))
    {
        var space = text.IndexOf(' ');
        if (space < 0) space = text.Length;
        var command = text[..space].ToLower();
        if (command.LastIndexOf('@') is > 0 and int at)
        {
            if (command[(at + 1)..].Equals(me.Username, StringComparison.OrdinalIgnoreCase))
                command = command[..at];
            else
                return;
        }
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
        case { CallbackQuery: { } callbackQuery }:
            await OnCallbackQuery(callbackQuery);
            break;
        default:
            Console.WriteLine($"Received unhandled update {update.Type}");
            break;
    }
}

async Task OnCallbackQuery(CallbackQuery callbackQuery)
{
    if (callbackQuery.Data.StartsWith("genre:") || callbackQuery.Data.StartsWith("country:")
                                                || callbackQuery.Data.StartsWith("year:") || callbackQuery.Data.StartsWith("next:")
                                                || callbackQuery.Data == "apply")
    {
        await SettingsCommandHandler.HandleSelectionAsync(bot, callbackQuery, dispatcher);
    }
    else
    {
        await dispatcher.DispatchAsync(callbackQuery.Data, "", bot, callbackQuery);
    }
}

async Task OnPollAnswer(PollAnswer pollAnswer)
{
    if (pollAnswer.User != null)
        await bot.SendMessage(pollAnswer.User.Id, $"You voted for option(s) id [{string.Join(',', pollAnswer.OptionIds)}]");
}
