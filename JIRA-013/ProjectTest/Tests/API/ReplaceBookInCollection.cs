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
    [NonParallelizable]
    [TestFixture, Category("ReplaceBookInCollection")]
    public class ReplaceBookInCollection : BaseTest
    {
        private BookService _bookService;

        public ReplaceBookInCollection()
        {
            _bookService = new BookService(ApiClient);
        }

        [Test, TestCaseSource(typeof(BookDataProvider), nameof(BookDataProvider.GetValidBookData))]
        public void ReplaceBookInCollectionSuccessfullyWithValidData(BookDataModel book)
        {
            AccountDataModel account = AccountData[book.AccountKey];
            string token = GetToken(account);

            ExtentReportHelpers.LogTestStep("Delete existing book from collection to ensure clean state");
            _bookService.DeleteBookFromCollection(account.UserId, book.Isbn, token);

            ExtentReportHelpers.LogTestStep("Add book to collection before replacement");
            _bookService.AddBookToCollection(
                new PostBookRequestDto
                {
                    UserId = account.UserId,
                    CollectionOfIsbns = new IsbnDto[]
                    {
                        new IsbnDto { Isbn = book.Isbn }
                    }
                }, token);

            ExtentReportHelpers.LogTestStep("Store data to clean up the replaced book after test");
            _bookService.StoreDataToDeleteBook(account.UserId, book.ReplaceIsbn, token);

            ExtentReportHelpers.LogTestStep("Send replace book request");
            var response = _bookService.ReplaceBookInCollection(book.Isbn, account.UserId, book.ReplaceIsbn, token);

            ExtentReportHelpers.LogTestStep("Verify response status code is 200 OK and response schema is valid");
            response
                .VerifyStatusCodeOk()
                .VerifySchema(FileConstant.UserDetailSchemaFilePath.GetAbsolutePath());

            ExtentReportHelpers.LogTestStep("Verify the replaced book ISBN exists in the response");
            response.Data!.Books
                .Should().Contain(b => b.Isbn == book.ReplaceIsbn);

            ExtentReportHelpers.LogTestStep("Verify the userId in response matches the request");
            response.Data.UserId.Should().BeEquivalentTo(account.UserId);
        }


        [Test, TestCaseSource(typeof(BookDataProvider), nameof(BookDataProvider.GetValidBookData))]
        public void ReplaceBookInCollectionUnSuccessfullyWithMissingRequestBodyData(BookDataModel book)
        {
            AccountDataModel account = AccountData[book.AccountKey];
            string token = GetToken(account);
            book.ReplaceIsbn = "";

            ExtentReportHelpers.LogTestStep("Delete existing book from collection to ensure clean state");
            _bookService.DeleteBookFromCollection(account.UserId, book.Isbn, token);

            ExtentReportHelpers.LogTestStep("Add initial book to collection");
            _bookService.AddBookToCollection(
                new PostBookRequestDto
                {
                    UserId = account.UserId,
                    CollectionOfIsbns = new IsbnDto[]
                    {
                        new IsbnDto { Isbn = book.Isbn }
                    }
                }, token);

            ExtentReportHelpers.LogTestStep("Store invalid ReplaceIsbn to cleanup later");
            _bookService.StoreDataToDeleteBook(account.UserId, book.ReplaceIsbn, token);

            ExtentReportHelpers.LogTestStep("Send replace book request with missing ReplaceIsbn");
            var response = _bookService.ReplaceBookInCollection(book.Isbn, account.UserId, book.ReplaceIsbn, token);

            ExtentReportHelpers.LogTestStep("Verify status code is 400 BadRequest and error message is correct");
            string message = response.ConvertToDynamicObject()?["message"] ?? string.Empty;
            response.VerifyStatusCodeBadRequest();
            message.Should().Be(MessageConstant.MissingRequestBodyMsg);
        }


        [Test, TestCaseSource(typeof(BookDataProvider), nameof(BookDataProvider.GetValidBookData))]
        public void ReplaceBookInCollectionUnSuccessfullyWithReplaceIsbnNonExisted(BookDataModel book)
        {
            AccountDataModel account = AccountData[book.AccountKey];
            string token = GetToken(account);
            string ReplaceIsbn = Guid.NewGuid().ToString(); 

            ExtentReportHelpers.LogTestStep("Delete existing book from collection to ensure clean state");
            _bookService.DeleteBookFromCollection(account.UserId, book.Isbn, token);

            ExtentReportHelpers.LogTestStep("Add initial book to collection");
            _bookService.AddBookToCollection(
                new PostBookRequestDto
                {
                    UserId = account.UserId,
                    CollectionOfIsbns = new IsbnDto[]
                    {
                        new IsbnDto { Isbn = book.Isbn }
                    }
                }, token);

            ExtentReportHelpers.LogTestStep("Store ReplaceIsbn for later cleanup");
            _bookService.StoreDataToDeleteBook(account.UserId, book.ReplaceIsbn, token);

            ExtentReportHelpers.LogTestStep("Send Replace Book request with non-existing ReplaceIsbn");
            var response = _bookService.ReplaceBookInCollection(book.Isbn, account.UserId, ReplaceIsbn, token);

            ExtentReportHelpers.LogTestStep("Verify status code is 400 BadRequest and message indicates ISBN is not available");
            string message = response.ConvertToDynamicObject()?["message"] ?? string.Empty;

            response.VerifyStatusCodeBadRequest();
            message.Should().Be(MessageConstant.BookIsbnNotAvailableMsg);
        }


        [TearDown]
        public void ClearDataAfterBookTest()
        {
            _bookService.DeleteCreateBookFromStorage();
        }
    }
}
