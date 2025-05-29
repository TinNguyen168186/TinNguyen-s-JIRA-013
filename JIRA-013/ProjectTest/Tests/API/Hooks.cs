using Core.Reports;
using Core.Utilities;

namespace ProjectTest.Tests
{
    [SetUpFixture]
    public class Hooks
    {
        private const string AppSettingsPath = "Configurations/appsettings.json";

        [OneTimeSetUp]
        public void GlobalSetUp()
        {
            TestContext.Progress.WriteLine("=====> Global OneTimeSetUp: Loading configuration...");
            ConfigurationUtils.ReadConfiguration(AppSettingsPath);
            
            string reportFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            Directory.CreateDirectory(reportFolderPath);

            string reportPath = Path.Combine(reportFolderPath, "ExtentReport.html");
            ExtentReportHelpers.InitReport(reportPath);
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            TestContext.Progress.WriteLine("=====> Global OneTimeTearDown");
            ExtentReportHelpers.Flush();
        }        
    }


}
