using UtilityBot.Interfaces;

namespace UtilityBot.Controllers;

internal class TxtMsgController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _memoryStorage;

    public TxtMsgController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
    {
        _telegramClient = telegramBotClient;
        _memoryStorage = memoryStorage;
    }

    public async Task Handle(Message message, CancellationToken ct)
    {

        if (message.Text != null)
        {
            if (message.Text == BotCommands.Start)
            {
                await ResponseTemplates.StartMessage(_telegramClient, message.Chat.Id, ct);
                return;
            }
            var session = _memoryStorage.GetSession(message.Chat.Id);
            var languageCode = session.LanguageCode;
            var funcCode = session.FuncCode;

            if (message.Text == BotCommands.Func)
            {
                await ResponseTemplates.Selectfunc(_telegramClient, message.Chat.Id, ct, languageCode);
                return;
            }

            if (languageCode == ResponseTemplates.FuncLanguageCode.None)
            {
                await ResponseTemplates.StartMessage(_telegramClient, message.Chat.Id, ct);
                return;
            }

            if (funcCode == ResponseTemplates.FuncLanguageCode.None)
            {
                await ResponseTemplates.Selectfunc(_telegramClient, message.Id, ct, languageCode);
                return;
            }

            if (funcCode == ResponseTemplates.FuncLanguageCode.FuncCountingCharacters)
            {
                var count = _memoryStorage.FuncCountingCharacters(message.Text);
                if (languageCode == ResponseTemplates.FuncLanguageCode.ru)
                {
                    await _telegramClient.SendMessage(message.Chat.Id, $"Количество символов в тексте: {count}", cancellationToken: ct);
                }
                else
                {
                    await _telegramClient.SendMessage(message.Chat.Id, $"Number of characters in the text: {count}", cancellationToken: ct);
                }

            }
            else if (funcCode == ResponseTemplates.FuncLanguageCode.FuncSumNum)
            {
                if (message.Text.All(c => char.IsDigit(c) || char.IsWhiteSpace(c)))
                {
                        var sum = _memoryStorage.FuncSumNum(message.Text);
                        if (languageCode == ResponseTemplates.FuncLanguageCode.ru)
                        {
                            await _telegramClient.SendMessage(message.Chat.Id, $"Сумма чисел в тексте: {sum}", cancellationToken: ct);
                        }
                        else
                        {
                            await _telegramClient.SendMessage(message.Chat.Id, $"The sum of the numbers in the text: {sum}", cancellationToken: ct);
                        }
                }
                else
                {
                    if (languageCode == ResponseTemplates.FuncLanguageCode.ru)
                    {
                        await _telegramClient.SendMessage(message.Chat.Id, $"Текст содержит недопустимые символы", cancellationToken: ct);
                    }
                    else
                    {
                        await _telegramClient.SendMessage(message.Chat.Id, $"The text contains invalid characters", cancellationToken: ct);
                    }
                }
            }

        }

        Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");

    }
}
