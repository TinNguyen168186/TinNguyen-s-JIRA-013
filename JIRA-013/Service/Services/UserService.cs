using Core.API;

using RestSharp;

using Service.ObjectDto.Request;
using Service.ObjectDto.Response;

namespace Service.Services
{
    public class UserService
    {
        private APIClient _client;

        public UserService(APIClient apiClient)
        {
            this._client = apiClient;
        }

        public RestResponse<UserResponseDto> GetDetailUser(string? userId, string token)
        {
            return _client.CreateRequest(String.Format(APIContant.GetDetialUserEndPoint, userId))
                .AddHeader("Content-Type", ContentType.Json)
                .AddAuthorizationHeader(token)
                .ExecuteGet<UserResponseDto>();
        }

        public RestResponse GenerateToken(string username, string password)
        {
            return _client.CreateRequest(APIContant.GenerateTokenEndPoint)
                .AddHeader("Accept", ContentType.Json)
                .AddHeader("Content-Type", ContentType.Json)
                .AddBody(new TokenRequestDto
                {
                    UserName = username,
                    Password = password
                })
                .ExecutePost();
        }

        public RestResponse TryHardGenerateToken(string username, string password, int loops)
        {
            int count = 0;
            RestResponse? result = null;

            while (count < loops)
            {
                result = GenerateToken(username, password);

                if (result.Content != null && result.Content.Contains("User authorized successfully."))
                {
                    return result;
                }

                Thread.Sleep(4000);
                count++;
            }

            throw new Exception("Can not get token from API" + (result?.Content ?? string.Empty));
        }

    }
}