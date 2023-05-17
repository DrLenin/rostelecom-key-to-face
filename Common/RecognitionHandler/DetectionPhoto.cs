namespace Common.RecognitionHandler;

public class DetectionPhoto
{
    private List<FaceData>? _faceList;
    private EigenFaceRecognizer _recognizer = null!;
 
    private readonly CascadeClassifier _haarCascade;
    private readonly TelegramHandler _telegramBot;
    
    public DetectionPhoto(TelegramHandler botClient)
    {
        _haarCascade = new CascadeClassifier(Config.HaarCascadePath);
        _telegramBot = botClient;
        UpdatePhoto();
    }

    public bool Recognizer()
    {
        var bgrFrame = new Image<Bgr, byte>( Config.ScreenshotExample + Config.ExampleElement);
        
        var grayFrame = bgrFrame.Convert<Gray, byte>();

        var faces = _haarCascade.DetectMultiScale(grayFrame, 1.2, 10,
            new Size(50, 50), new Size(200, 200));

        foreach (var face in faces)
        {
            if (_faceList!.Count == 0) break;

            var faceImage = bgrFrame.Copy(face).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);

            var number = _recognizer.Predict(faceImage).Label;

            var result = _faceList[number - 1].Access;

            var createRandomName = new Random();

            var filename = createRandomName.Next(0, 1000) + Config.ExampleElement;

            if (result)
            {
                faceImage.Save(Config.ScreenshotExampleTrue + filename);
                _telegramBot.SendAccessPhoto(Config.ChatIdTelegram, Config.ScreenshotExampleTrue, filename);
            }
            else
            {
                faceImage.Save(Config.ScreenshotExampleFalse + filename);
                _telegramBot.SendDontAccessPhoto(Config.ChatIdTelegram, Config.ScreenshotExampleFalse, filename);
            }

            return result;
        }

        return false;
    }

    public void UpdatePhoto()
    {
        _faceList = new List<FaceData>();

        using var readerFacesToAccess = new StreamReader(Config.FacesToAccessTextFile);
        using var readerFacesToDontAccess = new StreamReader(Config.FacesToDontAccessTextFile);
        
        while (readerFacesToDontAccess.ReadLine()! is { } namePhoto)
        {
            var faceInstance = new FaceData
            {
                FaceImage = new Image<Gray, byte>(Config.FacePhotosToDontAccessPath + namePhoto + Config.ImageFileExtension),
                Access = false
            };
            _faceList.Add(faceInstance);
        }
        
        while (readerFacesToAccess.ReadLine()! is { } namePhoto)
        {
            var faceInstance = new FaceData
            {
                FaceImage =  new Image<Gray, byte>(Config.FacePhotosToAccessPath + namePhoto + Config.ImageFileExtension),
                Access = true
            };
            _faceList.Add(faceInstance);
        }
        
        var imageList = new VectorOfMat();
        var labelList = new VectorOfInt();

        var i = 0;
        
        foreach (var face in _faceList)
        {
            imageList.Push(face.FaceImage!.Mat);
            labelList.Push(new [] {i ++});
        }
        readerFacesToAccess.Close();
       
        if (_faceList.Count <= 0) return;
        
        _recognizer = new EigenFaceRecognizer(_faceList.Count);
            
        _recognizer.Train(imageList, labelList);
    }
}