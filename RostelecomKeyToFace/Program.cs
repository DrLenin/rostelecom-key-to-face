CheckDirectory();

var rostelecom = new RostelecomDriver();
var telegramBot = new TelegramHandler(Config.TokenTelegram);
var detection = new DetectionPhoto(telegramBot);

telegramBot.StartReceiving(HandleUpdateAsync);

rostelecom.GoToUrl();
rostelecom.Authorization();

while (true)
{
    rostelecom.GetPhoto();

    if(detection.Recognizer())
        rostelecom.OpenDoor();
    
    File.Delete(Config.ScreenshotExample + Config.ExampleElement);
    
    Thread.Sleep(1000);
}

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if(update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
    {
        var message = update.CallbackQuery;
        var path = message!.Data!.Split('_');

        var flag = path[0] == nameof(Availability.Access);

        var screenshotExample = flag ? Config.ScreenshotExampleFalse : Config.ScreenshotExampleTrue;
        var facePhotosToPath = flag ? Config.FacePhotosToAccessPath : Config.FacePhotosToDontAccessPath;
        var facesToTextFile = flag ? Config.FacesToAccessTextFile : Config.FacesToDontAccessTextFile;
        
        var fileInfo = new FileInfo(screenshotExample + path[1]);
                
        if (fileInfo.Exists)
        {
            fileInfo.MoveTo(facePhotosToPath + path[1]);

            await using (var writer = new StreamWriter(facesToTextFile, true))
                await writer.WriteLineAsync("\n" + path[1][..^4]);

            detection.UpdatePhoto();
        }
            
        await botClient.SendTextMessageAsync(message.Message!.Chat.Id, Config.CommandCompleted);
    }
    else
    {
        var message = update.Message;
        await botClient.SendTextMessageAsync(message!.Chat, Config.CommandError);
    }
}

void CheckDirectory()
{
    var directors = new[]
    {
        Config.FacePhotosToAccessPath, Config.FacePhotosToDontAccessPath, Config.ScreenshotExample,
        Config.ScreenshotExampleFalse, Config.ScreenshotExampleTrue
    };

    foreach (var directory in directors)
    {
        var directoryInfo = new DirectoryInfo(directory);
        
        if (!directoryInfo.Exists)
            directoryInfo.Create();
    }
}