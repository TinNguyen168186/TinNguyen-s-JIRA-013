using Core.API;
using Core.Extensions;
using Core.Reports;
using Core.ShareData;
using Core.Utilities;

using Newtonsoft.Json;

using NUnit.Framework.Interfaces;

using ProjectTest.Models;
using ProjectTest.Tests.DataProviders;

using Service.Services;

using static Core.Reports.ExtentReportHelpers;



namespace ProjectTest.Tests.API
{
    [TestFixture, Parallelizable(ParallelScope.Fixtures)]
    public class BaseTest
    {
        protected Dictionary<string, AccountDataModel> AccountData;
        protected APIClient ApiClient;
        protected UserService UserService;
        protected BookService BookService;
        public BaseTest()
        {
            AccountData = AccountDataProvider.LoadAccountDataFile();

            var config = ConfigurationUtils.ReadConfiguration("Configurations/appsettings.json");
            var baseUrl = ConfigurationUtils.GetConfigurationByKey("TestURL", config);

            ApiClient = new APIClient(baseUrl);
            UserService = new UserService(ApiClient);
            BookService = new BookService(ApiClient);
        }

        [OneTimeSetUp]
        public void CreateTestForExtentReport()
        {
            var className = TestContext.CurrentContext.Test.ClassName ?? "UnknownTestClass";
            ExtentReportHelpers.CreateTest(className);
        }

        [SetUp]
        public void SetUp()
        {
            ExtentReportHelpers.CreateNode(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void TearDown()
        {
            string testName = TestContext.CurrentContext.Test.Name;
            string stacktrace = TestContext.CurrentContext.Result.StackTrace ?? "";

            var nunitStatus = TestContext.CurrentContext.Result.Outcome.Status;

            var customStatus = nunitStatus switch
            {
                TestStatus.Passed => TestResultStatus.Passed,
                TestStatus.Failed => TestResultStatus.Failed,
                TestStatus.Skipped => TestResultStatus.Skipped,
                TestStatus.Inconclusive => TestResultStatus.Inconclusive,
                _ => TestResultStatus.Inconclusive
            };

            ExtentReportHelpers.CreateTestResult(customStatus, stacktrace, testName);
        }

        public string GetToken(AccountDataModel account)
        {
            if (string.IsNullOrEmpty(account.UserId))
                throw new ArgumentException("account.UserId must not be null or empty");

            if (DataStorage.GetData(account.UserId) is null)
            {
                var response = UserService.TryHardGenerateToken(account.Username, account.Password, 3);
                response.VerifyStatusCodeOk();

                if (string.IsNullOrEmpty(response.Content))
                    throw new InvalidOperationException("Response content is null or empty.");

                var result = JsonConvert.DeserializeObject<dynamic>(response.Content)
                    ?? throw new InvalidOperationException("Failed to deserialize response content");

                DataStorage.SetData(account.UserId, "Bearer " + result["token"]);
            }

            return DataStorage.GetData(account.UserId) as string ?? string.Empty;
        }



    }
}