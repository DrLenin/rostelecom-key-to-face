namespace Common.Services;

public class TelegramHandler
{
    private readonly TelegramBotClient _botClient;
    
    public TelegramHandler(string token)
    {
        var botClient = new TelegramBotClient(token);

        _botClient = botClient;
    }

    public void StartReceiving(Func<ITelegramBotClient, Update, CancellationToken, Task> handleUpdateAsync)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, 
        };
        
        _botClient.StartReceiving(
            handleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            CancellationToken.None
        );
    }

    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));

        return Task.Delay(1, cancellationToken);
    }
    
    public async void SendAccessPhoto(string chatId, string image, string nameFile)
    {
        await using Stream stream = System.IO.File.OpenRead(image + nameFile);

        var replyMarkup = new InlineKeyboardMarkup(new[]
        {
            new InlineKeyboardButton("Запретить") { CallbackData = "DontAccess_" + nameFile }
        });
        
        await _botClient.SendPhotoAsync(chatId,  InputFile.FromStream(stream), replyMarkup: replyMarkup, caption: "Доступн разрешён:");
    }
    
    public async void SendDontAccessPhoto(string chatId, string image, string nameFile)
    {
        await using Stream stream = System.IO.File.OpenRead(image + nameFile);

        var replyMarkup = new InlineKeyboardMarkup(new[]
        {
            new InlineKeyboardButton("Разрешить") { CallbackData = "Access_" + nameFile }
        });
        
        await _botClient.SendPhotoAsync(chatId,  InputFile.FromStream(stream), replyMarkup: replyMarkup, caption: "Доступ запрещён:");
    }
}