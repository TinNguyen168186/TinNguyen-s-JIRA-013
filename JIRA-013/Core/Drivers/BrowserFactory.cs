using System;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace Core.Drivers
{
    public static class BrowserFactory
    {
        private static ThreadLocal<IWebDriver> _threadLocalWebDriver = new ThreadLocal<IWebDriver>();
        public static void InitDriver (string browserName)
        {
            switch (browserName.ToLower()) 
            {
                case "chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments ("test-type");
                    chromeOptions.AddArgument ("--no-sandbox");

                    _threadLocalWebDriver.Value = new ChromeDriver(chromeOptions);
                    break;

                case "edge":
                    new DriverManager().SetUpDriver(new EdgeConfig(), VersionResolveStrategy.MatchingBrowser);
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument ("--no-sandbox");
                    
                    _threadLocalWebDriver.Value = new EdgeDriver(edgeOptions);
                    break;

                case "firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig(), VersionResolveStrategy.MatchingBrowser);
                    var firefoxOptions = new FirefoxOptions();                    
                    firefoxOptions.AddArgument ("--no-sandbox");

                    _threadLocalWebDriver.Value = new FirefoxDriver(firefoxOptions);
                    break;

                default:
                    throw new ArgumentException("Not a valid driver");
            }
        }
    
        public static IWebDriver GetDriver()
        {
            if (_threadLocalWebDriver.Value == null)
            {
                throw new InvalidOperationException("WebDriver has not been initialized. Call InitDriver first.");
            }
            return _threadLocalWebDriver.Value;
        }

        public static void QuitDriver()
        {
            if (_threadLocalWebDriver.Value != null)
            {
                _threadLocalWebDriver.Value.Quit();
                _threadLocalWebDriver.Value.Dispose();
            }
        }
    }
}