namespace Common.PageHandler;

public class RostelecomDriver
{
    private readonly IWebDriver _driver;
    
    private const string Url = "https://b2c.passport.rt.ru/auth/realms/b2c/protocol/openid-connect/auth?client_id=lk_dmh&redirect_uri=https://sso.key.rt.ru/api/v1/oauth2/b2c/callback&response_type=code&state=aHR0cHM6Ly9rZXkucnQucnUvbWFpbi9zaWduaW4/dD0xNjg0MTAyMDA3NTM3";

    private const string ButtonStandardAuth = "//BUTTON[@id='standard_auth_btn']";
    private const string InputUserName = "//INPUT[@id='username']";
    private const string InputPassWord = "//INPUT[@id='password']";
    private const string ButtonAuthGo = "//BUTTON[@id='kc-login']";
    private const string Video = "//VIDEO[@preload='none']";
    private const string ButtonOpenDoor = "//BUTTON[@type='button'][text()='Открыть дверь']";

    public RostelecomDriver()
    {
        _driver = new ChromeDriver();
    }

    ~RostelecomDriver()
    {
        _driver.Dispose();
    }

    public void GoToUrl()
    {
        _driver.Navigate().GoToUrl(Url);
        Thread.Sleep(500);
    }

    public void Authorization()
    {
        _driver.FindElement(By.XPath(ButtonStandardAuth)).Click();
        Thread.Sleep(500);
        
        _driver.FindElement(By.XPath(InputUserName)).SendKeys(Config.Username);
        Thread.Sleep(500);
        
        _driver.FindElement(By.XPath(InputPassWord)).SendKeys(Config.Password);
        Thread.Sleep(500);
        
        _driver.FindElement(By.XPath(ButtonAuthGo)).Click();
        Thread.Sleep(2000);
    }
    
    public void GetPhoto()
    {
        var element = _driver.FindElement(By.XPath(Video));

        var theElementWidth = element.Size.Width;
        var theElementHeight = element.Size.Height;

        var theElementLocationX = element.Location.X;
        var theElementLocationY = element.Location.Y;

        var rectangle = new Rectangle(theElementLocationX, theElementLocationY, theElementWidth, theElementHeight);

        var ms = new MemoryStream(((ITakesScreenshot)_driver).GetScreenshot().AsByteArray);

        using var importFile = new Bitmap(ms);
        
        var cloneFile = importFile.Clone(rectangle, importFile.PixelFormat);
        
        cloneFile.Save( Config.ScreenshotExample + Config.ExampleElement);
    }

    public void OpenDoor()
    {
        _driver.FindElement(By.XPath(ButtonOpenDoor)).Click();
        Thread.Sleep(10000);
    }
}