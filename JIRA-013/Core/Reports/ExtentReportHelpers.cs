using System;

using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

using Core.Utilities;

namespace Core.Reports
{
    public class ExtentReportHelpers
    {
        private static ExtentReports? _extentManager;
        [ThreadStatic]

        private static ExtentTest? _extentTest;
        [ThreadStatic]

        private static ExtentTest? _node;

        public enum TestResultStatus
        {
            Passed,
            Failed,
            Skipped,
            Inconclusive
        }

        public static void InitReport(string reportPath)
        {
            ExtentSparkReporter htmlReporter = new ExtentSparkReporter(reportPath);

            _extentManager = new ExtentReports();
            _extentManager.AttachReporter(htmlReporter);
            _extentManager.AddSystemInfo("Environment", "Staging");
            _extentManager.AddSystemInfo("Browser", ConfigurationUtils.GetConfigurationByKey("Browser"));
        }

        public static void CreateTest(string testName)
        {
            if (_extentManager != null)
            {
                _extentTest = _extentManager.CreateTest(testName);
            }
            else
            {
                throw new InvalidOperationException("ExtentReports manager is not initialized. Call InitializeReport first.");
            }
        }

        public static void CreateNode(string nodeName)
        {
            if (_extentTest != null)
            {
                _node = _extentTest.CreateNode(nodeName);
            }
            else
            {
                throw new InvalidOperationException("ExtentTest is not initialized. Call CreateTest first.");
            }
        }

        public static void LogTestStep(string stepName)
        {
            _node?.Info(stepName);
        }

        public static void CreateTestResult(TestResultStatus status, string stacktrace, string testName)
        {
            Status logStatus;
            switch (status)
            {
                case TestResultStatus.Passed:
                    logStatus = Status.Pass;
                    _node?.Pass($"===> Test Name: {testName} - Status: {logStatus}");
                    break;
                    
                case TestResultStatus.Failed:
                    logStatus = Status.Fail;
                    _node?.Fail("#TestName: " + testName + "#Status: " + logStatus + stacktrace);
                    break;

                case TestResultStatus.Skipped:
                    logStatus = Status.Skip;
                    _node?.Skip($"===> Test Name: {testName} - Status: {logStatus}");
                    break;

                case TestResultStatus.Inconclusive:
                    logStatus = Status.Warning;
                    _node?.Warning($"===> Test Name: {testName} - Status: {logStatus}");
                    break;

                default:
                    logStatus = Status.Pass;
                    _node?.Pass($"===> Test Name: {testName} - Status: {logStatus}");
                    break;
            }
        }

        public static void Flush()
        {
            _extentManager?.Flush();
        }
    }
}