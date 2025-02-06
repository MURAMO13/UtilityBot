namespace UtilityBot.Controllers;
internal class InlineKeyboardController
{

    private readonly IStorage _memoryStorage;
    private readonly ITelegramBotClient _telegramClient;

    public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
    {
        _telegramClient = telegramBotClient;
        _memoryStorage = memoryStorage;
    }



    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        if (callbackQuery?.Data == null)
            return;

        switch (callbackQuery.Data)
        {
            case nameof(ResponseTemplates.FuncLanguageCode.ru):
                _memoryStorage.GetSession(callbackQuery.From.Id).LanguageCode = ResponseTemplates.FuncLanguageCode.ru;
                await SendLanguageSelectionMessage(callbackQuery.From.Id, "<b>Ваш язык Русский.</b>\n\nМожно поменять в главном меню.", ct);
                await ResponseTemplates.Selectfunc(_telegramClient, callbackQuery.From.Id, ct, ResponseTemplates.FuncLanguageCode.ru);
                break;

            case nameof(ResponseTemplates.FuncLanguageCode.en):
                _memoryStorage.GetSession(callbackQuery.From.Id).LanguageCode = ResponseTemplates.FuncLanguageCode.en;
                await SendLanguageSelectionMessage(callbackQuery.From.Id, "<b>Your language is English.</b>\n\nYou can change it in the main menu.", ct);
                await ResponseTemplates.Selectfunc(_telegramClient, callbackQuery.From.Id, ct, ResponseTemplates.FuncLanguageCode.en);
                break;

            case nameof(ResponseTemplates.FuncLanguageCode.FuncCountingCharacters):
                _memoryStorage.GetSession(callbackQuery.From.Id).FuncCode = ResponseTemplates.FuncLanguageCode.FuncCountingCharacters;
                await _telegramClient.SendMessage(callbackQuery.From.Id, "Отправьте текст для подсчёта символов!", cancellationToken: ct);
                break;

            case nameof(ResponseTemplates.FuncLanguageCode.FuncSumNum):
                _memoryStorage.GetSession(callbackQuery.From.Id).FuncCode = ResponseTemplates.FuncLanguageCode.FuncSumNum;
                await _telegramClient.SendMessage(callbackQuery.From.Id, "Введите числа через пробел, чтобы узнать их сумму.\nПример: 200 50", cancellationToken: ct);
                break;
        }

    }

    private async Task SendLanguageSelectionMessage(long chatId, string message, CancellationToken ct)
    {
        await _telegramClient.SendMessage(chatId, message, parseMode: ParseMode.Html, cancellationToken: ct);
    }





}
