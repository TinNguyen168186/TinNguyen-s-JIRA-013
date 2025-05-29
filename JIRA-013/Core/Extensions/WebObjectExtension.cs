using System;
using System.Collections.Generic;

using Core.Drivers;
using Core.Elements;
using Core.Utilities;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Core.Extensions
{
    public static class WebObjectExtension
    {
        static int WAIT_TIME_SECONDS = int.Parse(ConfigurationUtils.GetConfigurationByKey("WaitTimeSeconds"));
        public static IWebElement FindElement(this WebObject webObject)
        {
            var driver = BrowserFactory.GetDriver();

            try
            {
                return driver.FindElement(webObject.By);
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException($"Element '{webObject.Name}' with locator {webObject.By} was not found.");
            }
        }
        public static IList<IWebElement> FindElements(this WebObject webObject, By by)
        {
            var parent = webObject.FindElement();
            return parent.FindElements(by);
        }
        public static IList<IWebElement> FindElementsInside(this WebObject webObject, IWebElement parentElement)
        {
            try
            {
                return parentElement.FindElements(webObject.By);
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException($"Elements with locator {webObject.By} not found inside parent element.");
            }
        }
        public static IWebElement WaitForElementToBeVisible(this WebObject webObject)
        {
            var wait = new WebDriverWait(BrowserFactory.GetDriver(), TimeSpan.FromSeconds(WAIT_TIME_SECONDS));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(webObject.By));
        }

        public static IWebElement WaitForElementToBeClickable(this WebObject webObject)
        {
            var wait = new WebDriverWait(BrowserFactory.GetDriver(), TimeSpan.FromSeconds(WAIT_TIME_SECONDS));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(webObject.By));
        }
        public static bool IsElementDisplayed(this WebObject webObject)
        {
            var wait = new WebDriverWait(BrowserFactory.GetDriver(), TimeSpan.FromSeconds(WAIT_TIME_SECONDS));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(webObject.By)).Displayed;
        }
        public static void ScrollToElement(this WebObject webObject)
        {
            var driver = BrowserFactory.GetDriver();
            var element = webObject.FindElement();

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ block: 'center', behavior: 'instant' });", element);
        }
        public static void Tab(this WebObject webObject)
        {
            Actions actions = new Actions(BrowserFactory.GetDriver());
            actions.SendKeys(Keys.Tab).Perform();
        }
        public static void Click(this WebObject webObject)
        {
            var driver = BrowserFactory.GetDriver();

            var element = WaitForElementToBeClickable(webObject);
            webObject.ScrollToElement();

            element.Click();
        }
        public static void InputText(this WebObject webObject, string text)
        {
            var element = WaitForElementToBeVisible(webObject);
            webObject.ScrollToElement();
            element.Clear();
            Console.WriteLine($"Entering text: {text} to element: " + webObject.Name);
            element.SendKeys(text);
        }
        public static void SelectDropdownByText(this WebObject webObject, string text)
        {
            webObject.ScrollToElement();
            var element = WaitForElementToBeVisible(webObject);
            var selectElement = new SelectElement(element);
            selectElement.SelectByText(text);
        }
        public static void SelectDropdownByValue(this WebObject webObject, string value)
        {
            var element = WaitForElementToBeVisible(webObject);
            var selectElement = new SelectElement(element);
            selectElement.SelectByValue(value);
        }
        public static void SelectDropdownByIndex(this WebObject webObject, int index)
        {
            var element = WaitForElementToBeVisible(webObject);
            var selectElement = new SelectElement(element);
            selectElement.SelectByIndex(index);
        }
        public static IList<IWebElement> WaitForElementsToBeVisible(this WebObject webObject)
        {
            var wait = new WebDriverWait(BrowserFactory.GetDriver(), TimeSpan.FromSeconds(WAIT_TIME_SECONDS));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(webObject.By));
        }
        public static List<string> GetAllElementsText(this WebObject webObject)
        {
            var elements = webObject.WaitForElementsToBeVisible();

            var texts = new List<string>();

            foreach (var element in elements)
            {
                string text = element.Text.Trim();

                if (!string.IsNullOrEmpty(text))
                {
                    texts.Add(text);
                }
            }

            return texts;
        }
        public static string GetElementText(this WebObject webObject)
        {
            var element = webObject.WaitForElementToBeVisible();
            return element.Text.Trim();
        }
        
    }
}