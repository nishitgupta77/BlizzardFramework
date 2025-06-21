using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

namespace BlizzardTests.Utilities
{
    public static class ExtentReportManager
    {
        private static ExtentReports _extent;

        public static ExtentReports CreateReportInstance()
        {
            if (_extent == null)
            {
                var twoLevelsUp = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..",".."));
                var reportDir = Path.Combine(twoLevelsUp, "Reports");
                Directory.CreateDirectory(reportDir);

                var reportPath = Path.Combine(reportDir, "TestReport.html");
                var sparkReporter = new ExtentSparkReporter(reportPath);

                sparkReporter.Config.DocumentTitle = "Blizzard Test Report";
                sparkReporter.Config.ReportName = "Blizzard UI Automation";
                sparkReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Dark;

                _extent = new ExtentReports();
                _extent.AttachReporter(sparkReporter);

                _extent.AddSystemInfo("Environment", "QA");
                _extent.AddSystemInfo("Tester", "Nishit Gupta");
            }

            return _extent;
        }
    }
}