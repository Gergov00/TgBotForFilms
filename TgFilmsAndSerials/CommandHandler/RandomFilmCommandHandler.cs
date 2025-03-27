using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Services.Interfaces;

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
        var movie = await _movieService.GetRandom();
        var description = movie.Description != null ? movie.Description : "Отсутствует";
        string text = $"Случайное кино: {movie.Name}\nОписание: {description}";
        var photoUrl = movie.Poster.PreviewUrl;
        var media = new InputMediaPhoto(photoUrl)
        {
            Caption = text
        };
        
        await bot.EditMessageMediaAsync(
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