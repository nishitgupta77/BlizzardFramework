using AventStack.ExtentReports;
using BlizzardTests.Drivers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace BlizzardTests.Utilities
{
    public class TestFixture
    {
        private IWebDriver _driver;
        protected static ExtentReports Extent;
        protected ExtentTest Test;
        private ServiceProvider _serviceProvider;

        protected IConfiguration Configuration { get; private set; }


        protected IWebDriver Driver => _driver;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            Extent = ExtentReportManager.CreateReportInstance();
        }

        [SetUp]
        public void SetupTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddSingleton(Configuration);
            services.AddSingleton<WebDriverFactory>();
            services.AddSingleton(provider =>
            {
                var factory = provider.GetRequiredService<WebDriverFactory>();
                return factory.CreateDriver();
            });

            _serviceProvider = services.BuildServiceProvider();
            _driver = _serviceProvider.GetRequiredService<IWebDriver>();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            var testName = TestContext.CurrentContext.Test.Name;
            Test = Extent.CreateTest(testName);
        }

        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;

            var base64Screenshot = CaptureScreenshotAsBase64();

              switch (status)
              {
                case NUnit.Framework.Interfaces.TestStatus.Failed:
                    if (!string.IsNullOrEmpty(base64Screenshot))
                    {
                        Test.Fail($"Test failed: {message}")
                            .AddScreenCaptureFromBase64String(base64Screenshot, "Failure Screenshot");
                    }
                    else
                    {
                        Test.Fail($"Test failed: {message}");
                    }
                    break;

                case NUnit.Framework.Interfaces.TestStatus.Passed:
                    if (!string.IsNullOrEmpty(base64Screenshot))
                    {
                        Test.Pass("Test passed")
                            .AddScreenCaptureFromBase64String(base64Screenshot, "Passed Screenshot");
                    }
                    else
                    {
                        Test.Pass("Test passed");
                    }
                    break;

                default:
                    Test.Skip("Test skipped");
                    break;
              }

            _driver?.Dispose();
            _serviceProvider?.Dispose();
            Extent.Flush();
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            Extent.Flush();
        }
        protected string CaptureScreenshotAsBase64()
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                return screenshot.AsBase64EncodedString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error capturing screenshot: {ex.Message}");
                return null;
            }
        }
    }
}