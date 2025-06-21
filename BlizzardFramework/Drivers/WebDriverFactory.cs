using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Microsoft.Extensions.Configuration;

namespace BlizzardTests.Drivers
{
    public class WebDriverFactory
    {
        private readonly IConfiguration _configuration;

        public WebDriverFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IWebDriver CreateDriver()
        {
            string browser = _configuration["Browser"]?.ToLower();

            switch (browser)
            {
                case "chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig());

                    var chromeOptions = new ChromeOptions();

                    if (bool.TryParse(_configuration["Headless"], out bool isHeadless) && isHeadless)
                    {
                        chromeOptions.AddArgument("--headless=new"); 
                        chromeOptions.AddArgument("--disable-gpu");
                        chromeOptions.AddArgument("--no-sandbox");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                    }
                    return new ChromeDriver(chromeOptions);

                case "firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    return new FirefoxDriver();

                case "edge":
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    return new EdgeDriver();

                default:
                    throw new NotSupportedException($"Browser '{browser}' is not supported.");
            }
        }
    }
}