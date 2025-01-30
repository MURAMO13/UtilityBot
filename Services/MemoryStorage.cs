namespace UtilityBot.Services;

internal class MemoryStorage : IStorage
{
    private readonly ConcurrentDictionary<long, Session> _sessions;

    public MemoryStorage()
    {
        _sessions = new ConcurrentDictionary<long, Session>();
    }

    int IStorage.FuncCountingCharacters(string enteredText)
    {
        return enteredText.Length;
    }

    decimal IStorage.FuncSumNum(string enteredText)
    {
        var wordsarr = enteredText.Split(' ');

        decimal sum = 0;

        foreach (var word in wordsarr)
        {
            if (decimal.TryParse(word, out decimal number))
            {
                sum += number;
            }
        }
        return sum;
    }

    Session IStorage.GetSession(long chatId)
    {
        // Возвращаем сессию по ключу, если она существует
        if (_sessions.ContainsKey(chatId))
            return _sessions[chatId];

        // Создаем и возвращаем новую, если такой не было
        var newSession = new Session();
        _sessions.TryAdd(chatId, newSession);
        return newSession;
    }
}
