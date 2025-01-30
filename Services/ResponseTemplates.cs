namespace UtilityBot.Services;
internal static class ResponseTemplates
{
    internal enum FuncLanguageCode
    {
        None,
        FuncCountingCharacters,
        FuncSumNum,
        ru,
        en
    }
    public static async Task StartMessage(ITelegramBotClient _telegramClient,long chatId, CancellationToken ct)
    {
        var buttons = new List<InlineKeyboardButton[]>();
        buttons.Add(new[]
        {
              InlineKeyboardButton.WithCallbackData($" Русский" , $"{FuncLanguageCode.ru}"),
              InlineKeyboardButton.WithCallbackData($" English" , $"{FuncLanguageCode.en}")
        });

        await _telegramClient.SendMessage
            (
              chatId: chatId,
              text: $"<b>Выберите язык бота.</b> {Environment.NewLine}" +
                    $"{Environment.NewLine}Select the language of the bot.{Environment.NewLine}",
              parseMode: ParseMode.Html,
              replyMarkup: new InlineKeyboardMarkup(buttons),
              cancellationToken: ct
            );
    }

    public static async Task Selectfunc(ITelegramBotClient _telegramClient, long chatId, CancellationToken ct, FuncLanguageCode languageCode)
    {
        if (languageCode == FuncLanguageCode.None)
        {
            await StartMessage(_telegramClient, chatId, ct);
            return;
        }
        

        var buttons = new List<InlineKeyboardButton[]>();
        if (languageCode == ResponseTemplates.FuncLanguageCode.ru)
        {
            buttons.Add(new[]
            {
              InlineKeyboardButton.WithCallbackData($"Подсчитать количество символов",$"{FuncLanguageCode.FuncCountingCharacters}"),
              InlineKeyboardButton.WithCallbackData($"Подсчитать сумму чисел" , $"{FuncLanguageCode.FuncSumNum}")
            });

            await _telegramClient.SendMessage
            (
              chatId: chatId,
              text: $"<b>Выберите функцию:</b> {Environment.NewLine}",
              parseMode: ParseMode.Html,
              replyMarkup: new InlineKeyboardMarkup(buttons),
              cancellationToken: ct
            );
        }
        else 
        {
            buttons.Add(new[]
            {
              InlineKeyboardButton.WithCallbackData($"Count the number of characters", $"{FuncLanguageCode.FuncCountingCharacters}"),
              InlineKeyboardButton.WithCallbackData($"Calculate the sum of the numbers", $"{FuncLanguageCode.FuncSumNum}")

            });

            await _telegramClient.SendMessage
            (
              chatId: chatId,
              text: $"<b>Select the function:</b> {Environment.NewLine}",
              parseMode: ParseMode.Html,
              replyMarkup: new InlineKeyboardMarkup(buttons),
              cancellationToken: ct
            );
        }
        
        
    }
  
}
