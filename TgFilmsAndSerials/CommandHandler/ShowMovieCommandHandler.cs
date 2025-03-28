using Data.Entities;
using Data.Sorage;
using Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgFilmsAndSerials.Markup;


namespace TgFilmsAndSerials.CommandHandler;

public class ShowMovieCommandHandler: ICommandHandler
{
    private readonly IMovieService _movieService;
    private readonly StateSorage _stateSorage;

    public ShowMovieCommandHandler(IMovieService movieService, StateSorage stateSorage)
    {
        _stateSorage = stateSorage;
        _movieService = movieService;
    }

    public string Command => "/showmovie";
    public async Task HandleAsync(string? args, TelegramBotClient bot, CallbackQuery? callbackQuery)
    {
        if (int.TryParse(callbackQuery.Data["movie_id:".Length..], out var id))
        {
            var movie = await _movieService.GetById(id);

            var (caption, photoUrl, parseMode, inlineMarkup) = MovieMarkup.GetMovieMarkup(movie);
            
            await bot.SendPhoto(
                chatId: callbackQuery.Message.Chat.Id,
                photo: photoUrl,
                caption: caption,
                parseMode: parseMode,
                replyMarkup: inlineMarkup
            );
            
        }
    }

    public async Task HandleAsync(string? args, TelegramBotClient bot, Message? message)
    {
        IEnumerable<MovieInfoSimplified> movies = null;
        
        if(_stateSorage.State[message.From.Id] == "/search")
        {
            movies = await _movieService.GetByTitle(args);
        }

        if (movies == null)
        {
            await bot.SendMessage(message.Chat, $"По запросу \"{args}\" ничего не найдено");
        }
        else
        {
            var buttons = new List<InlineKeyboardButton[]>();

            foreach (var _movie in movies)
            {
                
                string callbackData = $"movie_id:{_movie.Id}";
                var button = new[] { InlineKeyboardButton.WithCallbackData(_movie.DisplayName, callbackData) };
                buttons.Add(button);
            }

            var inlineKeyboard = new InlineKeyboardMarkup(buttons);

            await bot.SendMessage(message.Chat, "Выберите фильм:", replyMarkup: inlineKeyboard);
        }
    }

}