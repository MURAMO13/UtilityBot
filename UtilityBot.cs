namespace UtilityBot;
internal class UtilityBot: BackgroundService
{
    private ITelegramBotClient _telegramClient;

    private InlineKeyboardController _inlinerKeyboardController;
    private TxtMsgController _txtMsgController;

    public UtilityBot(ITelegramBotClient telegramClient, InlineKeyboardController inlinerKeyboardController, TxtMsgController txtMsgController)
    {
        _telegramClient = telegramClient;
        _inlinerKeyboardController = inlinerKeyboardController;
        _txtMsgController = txtMsgController;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions() { AllowedUpdates = { } }, // Здесь выбираем, какие обновления хотим получать. В данном случае разрешены все  
            cancellationToken: stoppingToken);

        Console.WriteLine("Бот запущен");

        // Ожидание завершения задачи, не знаю правильно ли.
        //await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            await _inlinerKeyboardController.Handle(update.CallbackQuery, cancellationToken);
            return;
        }

        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            Console.WriteLine($"Получено сообщение {update.Message.Text}");
            await _txtMsgController.Handle(update.Message, cancellationToken);
            return;
        }

        //Default message  
        await _telegramClient.SendMessage(update.Message.Chat.Id, "Пока непонятно!", cancellationToken: cancellationToken);
    }

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Задаем сообщение об ошибке в зависимости от того, какая именно ошибка произошла
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        // Выводим в консоль информацию об ошибке
        Console.WriteLine(errorMessage+"\n");

        // Задержка перед повторным подключением
        Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
        Task.Delay(10000, cancellationToken);

        return Task.CompletedTask;
    }
}

