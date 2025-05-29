using Core.Extensions;
using Core.Reports;

using FluentAssertions;

using ProjectTest.Constants;
using ProjectTest.Models;
using ProjectTest.Tests.DataProviders;

namespace ProjectTest.Tests.API
{
    [TestFixture, Category("GetUser")]
    public class GetUserTest : BaseTest
    {
        [Test, TestCaseSource(typeof(AccountDataProvider), nameof(AccountDataProvider.GetValidAccount))]
        public void GetDetailUserSuccessfulWithValidData(AccountDataModel account)
        {
            ExtentReportHelpers.LogTestStep("Get token with valid account");
            string token = GetToken(account);

            ExtentReportHelpers.LogTestStep("Send request to get user detail");
            var response = UserService.GetDetailUser(account.UserId, token);

            ExtentReportHelpers.LogTestStep("Verify status code is 200 OK and response schema is valid");
            response
                .VerifyStatusCodeOk()
                .VerifySchema(FileConstant.UserDetailSchemaFilePath.GetAbsolutePath());

            ExtentReportHelpers.LogTestStep("Verify response data matches with expected account info");
            response.Data!.UserId.Should().BeEquivalentTo(account.UserId);
            response.Data.Username.Should().BeEquivalentTo(account.Username);
        }


        [Test, TestCaseSource(typeof(AccountDataProvider), nameof(AccountDataProvider.GetValidAccount))]
        public void GetUserUnsuccessfullyWithUnauthorizedToken(AccountDataModel account)
        {
            ExtentReportHelpers.LogTestStep("Send Get User request with unauthorized token");
            var response = UserService.GetDetailUser(account.UserId, token: Guid.NewGuid().ToString());

            ExtentReportHelpers.LogTestStep("Verify status code is 401 Unauthorized and error message is correct");
            string message = response.ConvertToDynamicObject()?["message"] ?? string.Empty;

            response.VerifyStatusCodeUnauthorized();
            message.Should().Be(MessageConstant.UnauthorizedMsg);
        }


        [Test, TestCaseSource(typeof(AccountDataProvider), nameof(AccountDataProvider.GetValidAccount))]
        public void GetUserUnsuccessfullyWithUserIdNonExisted(AccountDataModel account)
        {
            string token = GetToken(account);
            string fakeUserId = Guid.NewGuid().ToString();

            ExtentReportHelpers.LogTestStep("Send Get User request with non-existed userId");
            var response = UserService.GetDetailUser(userId: fakeUserId, token);

            ExtentReportHelpers.LogTestStep("Verify status code is 401 Unauthorized and error message is correct");
            string message = response.ConvertToDynamicObject()?["message"] ?? string.Empty;

            response.VerifyStatusCodeUnauthorized();
            message.Should().Be(MessageConstant.UserNotFoundMsg);
        }

    }
}
