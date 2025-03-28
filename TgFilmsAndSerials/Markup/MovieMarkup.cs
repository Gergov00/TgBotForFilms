using Data.Entities;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgFilmsAndSerials.Markup;

public static class MovieMarkup
{
    public static (string PhotoUrl, string Caption, ParseMode ParseMode, InlineKeyboardMarkup) GetMovieMarkup(MovieInfoSimplified movie)
    {
        string text = $"<b>{movie.DisplayName}</b>\n" +
                      $"<i>{movie.Description}</i>\n\n" +
                      $"<b>Год выпуска:</b> {movie.Year}\n" +
                      $"<b>Жанры:</b> {string.Join(", ", movie.Genres?.Select(g => g.Name) ?? new List<string>())}\n" +
                      $"<b>Страны:</b> {string.Join(", ", movie.Countries?.Select(c => c.Name) ?? new List<string>())}";
        var photoUrl = movie.Poster == null ? null : movie.Poster.PreviewUrl;
        var inlineMarkup = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
        {
            new[] { InlineKeyboardButton.WithCallbackData("В избраное", "/favourites") },
            new[] { InlineKeyboardButton.WithCallbackData("Дальше", "/random") },
            new[] { InlineKeyboardButton.WithCallbackData("Меню", "/menu") }
        });

        return (text, photoUrl, ParseMode.Html, inlineMarkup);
    }
}