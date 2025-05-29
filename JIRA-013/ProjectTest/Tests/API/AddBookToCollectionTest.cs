using Core.Extensions;
using Core.Reports;

using FluentAssertions;

using ProjectTest.Constants;
using ProjectTest.Models;
using ProjectTest.Tests.DataProviders;

using RestSharp;

using Service.ObjectDto.Request;
using Service.ObjectDto.Response;
using Service.Services;

namespace ProjectTest.Tests.API
{
    [TestFixture, Category("AddBookToCollection")]
    public class AddBookToCollectionTest : BaseTest
    {
        private BookService _bookService;

        public AddBookToCollectionTest()
        {
            _bookService = new BookService(ApiClient);
        }

        [Test, TestCaseSource(typeof(BookDataProvider), nameof(BookDataProvider.GetValidBookData))]
        public void AddBookToCollectionSuccessfullyWithValidData(BookDataModel book)
        {
            AccountDataModel account = AccountData[book.AccountKey];

            ExtentReportHelpers.LogTestStep("Get token for account");
            string token = GetToken(account);

            ExtentReportHelpers.LogTestStep("Send delete book request (cleanup before test)");
            _bookService.DeleteBookFromCollection(account.UserId, book.Isbn, token);

            ExtentReportHelpers.LogTestStep("Send add book request to API");
            RestResponse<PostBookResponseDto> response = _bookService.AddBookToCollection(
                new PostBookRequestDto
                {
                    UserId = account.UserId,
                    CollectionOfIsbns = new IsbnDto[]
                    {
                        new IsbnDto { Isbn = book.Isbn }
                    }
                },
                token
            );

            ExtentReportHelpers.LogTestStep("Store book data for cleanup after test");
            _bookService.StoreDataToDeleteBook(account.UserId, book.Isbn, token);

            ExtentReportHelpers.LogTestStep("Verify the response status code");
            response.VerifyStatusCodeCreated();

            ExtentReportHelpers.LogTestStep("Verify the response body matches the schema");
            response.VerifySchema(FileConstant.AddBookToCollectionSchemaFilePath.GetAbsolutePath());

            ExtentReportHelpers.LogTestStep("Verify response contains the added book");
            response.Data!.Books
                .Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new PostBookIsbn { Isbn = book.Isbn ?? string.Empty });
        }


        [Test, TestCaseSource(typeof(BookDataProvider), nameof(BookDataProvider.GetValidBookData))]
        public void AddBookToCollectionUnSuccessfullyWithUserIdNonExisted(BookDataModel book)
        {
            AccountDataModel account = AccountData[book.AccountKey];

            ExtentReportHelpers.LogTestStep("Get token for account");
            string token = GetToken(account);

            ExtentReportHelpers.LogTestStep("Send delete book request to ensure clean state before test");
            _bookService.DeleteBookFromCollection(account.UserId, book.Isbn, token);

            ExtentReportHelpers.LogTestStep("Generate a random non-existent userId");
            string userId = Guid.NewGuid().ToString();

            ExtentReportHelpers.LogTestStep("Send add book request with non-existent userId");
            RestResponse<PostBookResponseDto> response = _bookService.AddBookToCollection(
                new PostBookRequestDto
                {
                    UserId = userId,
                    CollectionOfIsbns = new IsbnDto[]
                    {
                        new IsbnDto { Isbn = book.Isbn }
                    }
                }, token);
             _bookService.StoreDataToDeleteBook(account.UserId, book.Isbn, token);

            ExtentReportHelpers.LogTestStep("Extract message from response body");
            string message = response.ConvertToDynamicObject()?["message"] ?? string.Empty;

            ExtentReportHelpers.LogTestStep("Verify the response status code isUnauthorized");
            response.VerifyStatusCodeUnauthorized();

            ExtentReportHelpers.LogTestStep("Verify the error message is 'UserId is not correct'");
            message.Should().Be(MessageConstant.UserIdNotCorrect);
        }

        [Test, TestCaseSource(typeof(BookDataProvider), nameof(BookDataProvider.GetValidBookData))]
        public void AddBookToCollectionUnSuccessfullyWithUnauthorizedToken(BookDataModel book)
        {
            AccountDataModel account = AccountData[book.AccountKey];

            ExtentReportHelpers.LogTestStep("Get valid token and delete book from collection to ensure clean state");
            _bookService.DeleteBookFromCollection(account.UserId, book.Isbn, GetToken(account));

            ExtentReportHelpers.LogTestStep("Generate random token to simulate unauthorized access");
            string unauthorizedToken = Guid.NewGuid().ToString();

            ExtentReportHelpers.LogTestStep("Send add book request with unauthorized token");
            RestResponse<PostBookResponseDto> response = _bookService.AddBookToCollection(
                new PostBookRequestDto
                {
                    UserId = account.UserId,
                    CollectionOfIsbns = new IsbnDto[]
                    {
                        new IsbnDto { Isbn = book.Isbn }
                    }
                },
                token: unauthorizedToken);

            ExtentReportHelpers.LogTestStep("Extract message from response");
            string message = response.ConvertToDynamicObject()?["message"] ?? string.Empty;

            ExtentReportHelpers.LogTestStep("Verify the response status code is Unauthorized");
            response.VerifyStatusCodeUnauthorized();

            ExtentReportHelpers.LogTestStep($"Verify the response message is '{MessageConstant.UnauthorizedMsg}'");
            message.Should().Be(MessageConstant.UnauthorizedMsg);
        }


        [TearDown]
        public void ClearDataAfterBookTest()
        {
            _bookService.DeleteCreateBookFromStorage();
        }
    }
}
