using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Services.Interfaces;
using Telegram.Bot.Types.Enums;

namespace TgFilmsAndSerials.CommandHandler;

public class RandomFilmCommandHandler: ICommandHandler
{
    private readonly IMovieService _movieService;
    
    public RandomFilmCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }
    public string Command => "/random";

    public async Task HandleAsync(TelegramBotClient bot, CallbackQuery callbackQuery, string args)
    {
        var movie = await _movieService.GetRandomByFilter(null);
        var description = movie.Description != null ? movie.Description : "Отсутствует";
        string text = $"<b>{movie.DisplayName}</b>\n" +
                      $"<i>{movie.Description}</i>\n\n" +
                      $"<b>Год выпуска:</b> {movie.Year}\n" +
                      $"<b>Жанры:</b> {string.Join(", ", movie.Genres?.Select(g => g.Name) ?? new List<string>())}\n" +
                      $"<b>Страны:</b> {string.Join(", ", movie.Countries?.Select(c => c.Name) ?? new List<string>())}";
        var photoUrl = movie.Poster == null ? null : movie.Poster.PreviewUrl;
        var media = new InputMediaPhoto(photoUrl ?? "https://sun9-8.userapi.com/impg/TtcOpvkIQwxHfX6LU1o2VPsDtwyvOuOVA35Niw/1dUroHYP1Zw.jpg?size=800x800&quality=95&sign=c62d8f8489c8c98870da483314563fae&c_uniq_tag=tk935dMf9xLRR3sAoCJNfa6SV0uw92EDi2axBxnevso&type=album")
        {
            Caption = text,
            ParseMode = ParseMode.Html, 
        };
        
        await bot.EditMessageMedia(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            media: media,
            replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new [] { InlineKeyboardButton.WithCallbackData("В избраное", "/favourites") },
                new [] { InlineKeyboardButton.WithCallbackData("Дальше", "/random") },
                new [] { InlineKeyboardButton.WithCallbackData("Меню", "/menu") }
            })
        );
    }
}