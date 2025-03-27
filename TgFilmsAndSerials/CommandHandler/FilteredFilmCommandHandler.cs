using Data.Entities;
using Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgFilmsAndSerials.CommandHandler;
using TgFilmsAndSerials.Markup;

public class FilteredFilmCommandHandler : ICommandHandler
{
    private readonly IMovieService _movieService;

    public FilteredFilmCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public string Command => "/filter";

    public async Task HandleAsync(TelegramBotClient bot, CallbackQuery callbackQuery, string args)
    {
        var parts = args.Split('|');
        var filter = new UserFilter
        {
            Genres = parts[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            Countries = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            Year = int.TryParse(parts[2], out var y) ? y : null
        };

        var movie = await _movieService.GetRandomByFilter(filter);
        if (movie == null)
        {
            await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Фильм не найден по вашему фильтру.");
            return;
        }

        var text = $"<b>{movie.DisplayName}</b>\n" +
                   $"<i>{movie.Description ?? "Описание отсутствует"}</i>\n\n" +
                   $"<b>Год:</b> {movie.Year}\n" +
                   $"<b>Жанры:</b> {string.Join(", ", movie.Genres.Select(g => g.Name))}\n" +
                   $"<b>Страны:</b> {string.Join(", ", movie.Countries.Select(c => c.Name))}";

        var media = new InputMediaPhoto(movie.Poster?.PreviewUrl ?? "https://defaultimage.jpg")
        {
            Caption = text,
            ParseMode = ParseMode.Html
        };

        await bot.EditMessageMediaAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, media,
            replyMarkup: MenuMarkup.GetMainMenuMarkup());
    }
}