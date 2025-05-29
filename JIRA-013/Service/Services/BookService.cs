using Core.API;
using Core.ShareData;

using RestSharp;

using Service.ObjectDto.Request;
using Service.ObjectDto.Response;

namespace Service.Services
{
    public class BookService
    {
        private readonly APIClient _client;

        public BookService(APIClient apiClient)
        {
            this._client = apiClient;
        }

        public RestResponse<PostBookResponseDto> AddBookToCollection(PostBookRequestDto data, string token)
        {
            return _client.CreateRequest(APIContant.AddBookToCollectionEndPoint)
                .AddHeader("Accept", ContentType.Json)
                .AddHeader("Content-Type", ContentType.Json)
                .AddAuthorizationHeader(token)
                .AddBody(data)
                .ExecutePost<PostBookResponseDto>();
        }

        public RestResponse DeleteBookFromCollection(string userId, string isbn, string token)
        {
            return _client.CreateRequest(APIContant.DeleteBookFromCollectionEndPoint)
            .AddHeader("Accept", ContentType.Json)
            .AddHeader("Content-Type", ContentType.Json)
            .AddAuthorizationHeader(token)
            .AddBody(new DeleteBookRequestDto
            {
                UserId = userId,
                Isbn = isbn
            })
            .ExecuteDelete();
        }

        public void StoreDataToDeleteBook(string userId, string isbn, string token)
        {
            DataStorage.SetData("hasCreatedBook", true);
            DataStorage.SetData("userId", userId);
            DataStorage.SetData("isbn", isbn );
            DataStorage.SetData("token", token);
        }

        public void DeleteCreateBookFromStorage()
        {
            var createdBookFlag = DataStorage.GetData("hasCreatedBook");
            if (createdBookFlag is bool hasCreatedBook && hasCreatedBook)
            {
                var userId = DataStorage.GetData("userId") as string;
                var isbn = DataStorage.GetData("isbn") as string;
                var token = DataStorage.GetData("token") as string;

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(isbn) && !string.IsNullOrEmpty(token))
                {
                    DeleteBookFromCollection(userId, isbn, token);
                }
            }
        }

        public RestResponse<UserResponseDto> ReplaceBookInCollection(string isbn, string userId, string replaceIsbn, string token)
        {
            return _client.CreateRequest(String.Format(APIContant.ReplaceBookInCollectionEndPoint, isbn))
            .AddHeader("Accept", ContentType.Json)
            .AddHeader("Content-Type", ContentType.Json)
            .AddAuthorizationHeader(token)
            .AddBody(new PutBookRequestDto
            {
                UserId = userId,    
                Isbn = replaceIsbn 
            })
            .ExecutePut<UserResponseDto>();
        }
    }
}