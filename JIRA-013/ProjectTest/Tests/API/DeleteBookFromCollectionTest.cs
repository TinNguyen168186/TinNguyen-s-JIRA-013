using System.Net;

using Core.Extensions;
using Core.Reports;

using FluentAssertions;

using ProjectTest.Constants;
using ProjectTest.Models;
using ProjectTest.Tests.DataProviders;

using Service.ObjectDto.Request;
using Service.Services;

namespace ProjectTest.Tests.API
{
    [TestFixture, Category("DeleteBookFromCollection")]
    public class DeleteBookFromCollectionTest : BaseTest
    {
        private BookService _bookService;

        public DeleteBookFromCollectionTest()
        {
            _bookService = new BookService(ApiClient);
        }

        [Test, TestCaseSource(typeof(BookDataProvider), nameof(BookDataProvider.GetValidBookData))]
        public void DeleteBookFromCollectionSuccessfullyWithValidData(BookDataModel data)
        {
            AccountDataModel account = AccountData[data.AccountKey];
            string token = GetToken(account);
            _bookService.DeleteBookFromCollection(account.UserId, data.Isbn, token);

            ExtentReportHelpers.LogTestStep("Add book to collection to prepare for deletion");
            _bookService.AddBookToCollection(
            new PostBookRequestDto
            {
                UserId = account.UserId,
                CollectionOfIsbns = new IsbnDto[]
                {
                        new IsbnDto { Isbn = data.Isbn }
                }

            }, token);

            ExtentReportHelpers.LogTestStep("Send Delete Book request");
            var response = _bookService.DeleteBookFromCollection(account.UserId, data.Isbn, token);

            ExtentReportHelpers.LogTestStep("Verify status code is 204 NoContent");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }


        [Test, TestCaseSource(typeof(BookDataProvider), nameof(BookDataProvider.GetValidBookData))]
        public void DeleteBookFromCollectionSuccessfullyWithUnauthorizedToken(BookDataModel data)
        {
            AccountDataModel account = AccountData[data.AccountKey];
            string token = GetToken(account);
            _bookService.DeleteBookFromCollection(account.UserId, data.Isbn, token);

            ExtentReportHelpers.LogTestStep("Add book to collection to prepare for unauthorized deletion attempt");
            _bookService.AddBookToCollection(
            new PostBookRequestDto
            {
                UserId = account.UserId,
                CollectionOfIsbns = new IsbnDto[]
                {
                        new IsbnDto { Isbn = data.Isbn }
                }

            }, token);

            var invalidToken = Guid.NewGuid().ToString();

            ExtentReportHelpers.LogTestStep("Send Delete Book request with unauthorized token");
            var response = _bookService.DeleteBookFromCollection(account.UserId, data.Isbn, invalidToken);

            ExtentReportHelpers.LogTestStep("Verify status code is 401 Unauthorized and error message is correct");
            string message = response.ConvertToDynamicObject()?["message"] ?? string.Empty;
            response.VerifyStatusCodeUnauthorized();
            message.Should().Be(MessageConstant.UnauthorizedMsg);
        }


        [Test, TestCaseSource(typeof(BookDataProvider), nameof(BookDataProvider.GetValidBookData))]
        public void DeleteBookFromCollectionSuccessfullyWithIsbnNonExisted(BookDataModel data)
        {
            AccountDataModel account = AccountData[data.AccountKey];
            string token = GetToken(account);
            _bookService.DeleteBookFromCollection(account.UserId, data.Isbn, token);

            string isbn = Guid.NewGuid().ToString(); 

            ExtentReportHelpers.LogTestStep("Add book to collection to prepare test data");
            _bookService.AddBookToCollection(
            new PostBookRequestDto
            {
                UserId = account.UserId,
                CollectionOfIsbns = new IsbnDto[]
                {
                        new IsbnDto { Isbn = data.Isbn }
                }

            }, token);

            ExtentReportHelpers.LogTestStep("Send Delete Book request with non-existent ISBN");
            var response = _bookService.DeleteBookFromCollection(account.UserId, isbn, token);

            ExtentReportHelpers.LogTestStep("Verify status code is 400 Bad Request and error message is correct");
            string message = response.ConvertToDynamicObject()?["message"] ?? string.Empty;
            response.VerifyStatusCodeBadRequest();
            message.Should().Be(MessageConstant.UserIsbnNotAvailableMsg);
        }

    }
}
