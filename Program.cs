namespace UtilityBot;
internal class Program
{
    internal static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Объект, отвечающий за постоянный жизненный цикл приложения
        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
            .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
            .Build(); // Собираем

        Console.WriteLine("Сервис запущен");
        // Запускаем сервис
        await host.RunAsync();
        Console.WriteLine("Сервис остановлен");
    }

    static void ConfigureServices(IServiceCollection services)
    {
        AppSettings appSettings = BuildAppSettings();
        services.AddSingleton(appSettings);

        services.AddTransient<InlineKeyboardController>();
        services.AddTransient<TxtMsgController>();


        // Регистрируем объект TelegramBotClient c токеном подключения
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
        services.AddSingleton<IStorage, MemoryStorage>();
        // Регистрируем постоянно активный сервис бота
        services.AddHostedService<UtilityBot>();
    }

    static AppSettings BuildAppSettings()
    {
        return new AppSettings()
        {
            BotToken = "##################################"
        };
    }
}
