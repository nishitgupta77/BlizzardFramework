using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BlizzardFramework.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private IWebElement SummerSaleBanner => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[contains(text(),'Save up to 67%')]")));
        private IWebElement ShopNowButton => _driver.FindElement(By.XPath("//blz-button[@id='masthead_button_0_0']"));
        private IWebElement WarcraftMenu => _driver.FindElement(By.XPath("//blz-nav-dropdown[@id='blz-nav-wow']"));
        private IWebElement DiabloMenu => _driver.FindElement(By.XPath("//blz-nav-dropdown[@id='blz-nav-diablo']"));
        private IWebElement OverwatchMenu => _driver.FindElement(By.XPath("//blz-nav-dropdown[@id='blz-nav-overwatch']"));
        private IWebElement StarCraftMenu => _driver.FindElement(By.XPath("//blz-nav-dropdown[@id='blz-nav-starcraft']"));
        private IWebElement AboutMenu => _driver.FindElement(By.XPath("//blz-nav-dropdown[@id='blz-nav-about']"));
        public IWebElement DownloadButton => _driver.FindElement(By.XPath("//*[@id='blz-nav-download-battlenet']"));
        public IWebElement FeaturedGamesSection => _driver.FindElement(By.XPath("//h2[contains(text(), 'Featured Games')]"));
        public IReadOnlyCollection<IWebElement> FeaturedGameCards => _driver.FindElements(By.XPath("(//*[@class='ProductGrid mobileSlider'])[1]//blz-game-card[@class='DesktopCard ']"));

        public void GoTo()
        {
            _driver.Navigate().GoToUrl("https://www.blizzard.com/en-us/");
        }

        public string GetTitle()
        {
            return _driver.Title;
        }

        public void ClickShopNow()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", ShopNowButton);

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(ShopNowButton));

            js.ExecuteScript("arguments[0].click();", ShopNowButton);
        }

        public bool IsRedirectedToSale()
        {
            return _driver.Url.Contains("shop");
        }
        public void HoverOverMenu(string menu)
        {
            var actions = new OpenQA.Selenium.Interactions.Actions(_driver);

            switch (menu.ToLower())
            {
                case "warcraft":
                    actions.MoveToElement(WarcraftMenu).Perform();
                    break;
                case "diablo":
                    actions.MoveToElement(DiabloMenu).Perform();
                    break;
                case "overwatch":
                    actions.MoveToElement(OverwatchMenu).Perform();
                    break;
                case "starcraft":
                    actions.MoveToElement(StarCraftMenu).Perform();
                    break;
                case "about":
                    actions.MoveToElement(AboutMenu).Perform();
                    break;
                default:
                    throw new ArgumentException("Invalid menu option provided.");
            }
        }
        public List<string> GetDropdownItems(string menuName)
        {
            HoverOverMenu(menuName);
            Thread.Sleep(1000); // optional, better to use WebDriverWait

            var dropdownItems = _driver.FindElements(By.XPath($"//blz-nav-dropdown[@text='{menuName}']//blz-nav-link"));
            return dropdownItems.Select(item => item.Text.Trim()).ToList();
        }
        public void ClickDownloadButton()
        {
            DownloadButton.Click();
        }
        public bool IsDownloadPage()
        {
            return _driver.Url.Contains("download");
        }
        public List<string> GetFeaturedGameTitles()
        {
            return FeaturedGameCards
                .Select(card => card.FindElement(By.XPath(".//h3")).Text.Trim())
                .ToList();
        }
        public bool IsFeaturedGameCardComplete(IWebElement card)
        {
            try
            {
                var hasTitle = card.FindElement(By.XPath(".//h3")).Displayed;
                var hasImage = card.FindElement(By.XPath(".//blz-video")).Displayed;
                var hasLink = card.FindElement(By.XPath(".//blz-icon-group")).Displayed;

                return hasTitle && hasImage && hasLink;
            }
            catch (NoSuchElementException)
            {
                return false;
            }

        }
    }
}
