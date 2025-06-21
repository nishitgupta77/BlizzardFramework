using BlizzardFramework.Pages;
using BlizzardTests.Utilities;
using Microsoft.Extensions.Configuration;

namespace BlizzardFramework.Tests
{
    public class HomePageTests : TestFixture
    {
        private HomePage _homePage;

        [SetUp]
        public void SetUp()
        {
            _homePage = new HomePage(Driver);
        }

        [Test]
        public void Summer_Sale_Banner_IsVisible_And_Clickable()
        {
            Test.Info("Test started: SummerSaleBannerIsVisibleAndClickable");

            _homePage.GoTo();
            Test.Info("Navigated to Blizzard homepage");

            Assert.AreEqual(_homePage.GetTitle(), "Blizzard Entertainment");
            Test.Pass("Verified page title");

            _homePage.ClickShopNow();
            Test.Info("Clicked on 'Shop Now' button in summer sale banner");

            Assert.That(_homePage.IsRedirectedToSale(), "Redirection to sale page failed.");
            Test.Pass("Verified redirection to summer sale page");
        }

        [Test]
        public void Check_All_Nav_Menu_Opens_Successfully()
        {
            Test.Info("Test started: CheckAllNavMenuOpensSuccessfully");

            _homePage.GoTo();
            Test.Info("Navigated to Blizzard homepage");

            var menuNames = new List<string> { "Warcraft", "Diablo", "Overwatch", "About" };

            foreach (var menu in menuNames)
            {
                Test.Info($"Validating dropdown menu: {menu}");
                AssertDropdownItemsMatch(menu);
                Test.Pass($"Dropdown for '{menu}' matched expected items");
            }
        }

        [Test]
        public void Download_Button_Navigates_To_Download_Page()
        {
            Test.Info("Test started: DownloadButtonNavigatesToDownloadPage");

            _homePage.GoTo();
            Test.Info("Navigated to Blizzard homepage");

            _homePage.ClickDownloadButton();
            Test.Info("Clicked on the Download button");

            Assert.IsTrue(_homePage.IsDownloadPage(), "Clicking the Download button did not navigate to the download page.");
            Test.Pass("Successfully navigated to download page");
        }

        [Test]
        public void Featured_Games_Section_Is_Visible_With_ValidCards()
        {
            Test.Info("Test started: FeaturedGamesSectionIsVisibleWithValidCards");

            _homePage.GoTo();
            Test.Info("Navigated to Blizzard homepage");

            Assert.IsTrue(_homePage.FeaturedGamesSection.Displayed, "Featured Games section is not visible.");
            Test.Pass("Featured Games section is visible");

            var cards = _homePage.FeaturedGameCards;
            Assert.IsTrue(cards.Count > 0, "No featured game cards were found.");
            Test.Info($"Found {cards.Count} featured game cards");

            foreach (var card in cards)
            {
                Assert.IsTrue(_homePage.IsFeaturedGameCardComplete(card), "A featured game card is incomplete (missing title, image, or link).");
            }

            Test.Pass("All featured game cards are valid and complete");
        }

        [Test]
        public void Featured_Games_List_Matches_Expected()
        {
            Test.Info("Test started: FeaturedGamesListMatchesExpected");

            _homePage.GoTo();
            Test.Info("Navigated to Blizzard homepage");

            var actualTitles = _homePage.GetFeaturedGameTitles();
            Test.Info($"Actual featured game titles: {string.Join(", ", actualTitles)}");

            var expectedTitles = Configuration
                .GetSection("GameCards")
                .Get<List<string>>();

            CollectionAssert.IsSubsetOf(expectedTitles, actualTitles, "Featured games list does not match expected.");
            Test.Pass("Featured game titles match expected list");
        }

        private void AssertDropdownItemsMatch(string menuName)
        {
            _homePage.HoverOverMenu(menuName);
            Test.Info($"Hovered over menu: {menuName}");

            var actualItems = _homePage.GetDropdownItems(menuName);
            Test.Info($"Actual items: {string.Join(", ", actualItems)}");

            var expectedItems = Configuration
                .GetSection($"ExpectedMenus:{menuName}")
                .Get<List<string>>();

            CollectionAssert.AreEquivalent(
                expectedItems ?? new List<string>(),
                actualItems,
                $"Dropdown items for '{menuName}' do not match expected list.");
        }
    }
}
