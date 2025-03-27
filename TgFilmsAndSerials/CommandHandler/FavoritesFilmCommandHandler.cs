using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgFilmsAndSerials.CommandHandler;

public class FavoritesFilmCommandHandler : ICommandHandler
{
    
    public string Command => "/favourites";

    public async Task HandleAsync(TelegramBotClient bot, CallbackQuery callbackQuery, string args)
    {
        await bot.EditMessageText(
            chatId: callbackQuery.Message.Chat,
            messageId: callbackQuery.Message.Id,
            text: "Ваши избраные фильмы и сериалы",
            replyMarkup: new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("Фильм 1"), ],
                [InlineKeyboardButton.WithCallbackData("Фильм 2"), ],
                [InlineKeyboardButton.WithCallbackData("Меню", "/menu"), ]
            });
            
    }
}

