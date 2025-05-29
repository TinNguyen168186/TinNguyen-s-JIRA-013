using System;

using Core.Drivers;
using Core.Utilities;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Elements.Alert
{
    public class AlertHelper
    {
        static int WAIT_TIME_SECONDS = int.Parse(ConfigurationUtils.GetConfigurationByKey("WaitTimeSeconds"));
        public static IAlert WaitForAlertToBeVisible()
        {
            var wait = new WebDriverWait(BrowserFactory.GetDriver(), TimeSpan.FromSeconds(WAIT_TIME_SECONDS));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
        }
        public static void AcceptAlert()
        {
            var alert = WaitForAlertToBeVisible();
            alert.Accept();
        }
        public static string? GetAlertText()
        {
            return WaitForAlertToBeVisible().Text;
        }
    }
}