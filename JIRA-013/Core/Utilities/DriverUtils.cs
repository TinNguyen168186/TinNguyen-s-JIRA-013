using Core.Drivers;

namespace Core.Utilities
{
    public static class DriverUtils
    {
        public static void GoToUrl(string url)
        {
            BrowserFactory.GetDriver().Navigate().GoToUrl(url);
        }

        public static void MaximizeWindow()
        {
            BrowserFactory.GetDriver().Manage().Window.Maximize();
        }
    }
}