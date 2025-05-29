using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using RestSharp.Serializers.NewtonsoftJson;

namespace Core.API
{
    public class APIClient
    {
        private readonly RestClient _client;

        public RestRequest Request;

        private string _baseUrl = string.Empty;
        public APIClient(RestClient client)
        {
            _client = client;
            Request = new RestRequest();
        }

        public APIClient(string url)
        {
            _baseUrl = url;
            var options = new RestClientOptions(url);

            _client = new RestClient(options, configureSerialization: s => s.UseNewtonsoftJson());
            Request = new RestRequest();
        }

        private APIClient(RestClientOptions options)
        {
            _client = new RestClient(options, configureSerialization: s => s.UseNewtonsoftJson());
            Request = new RestRequest();
        }

        public APIClient SetBasicAuthentication(string username, string password)
        {
            var options = new RestClientOptions(_baseUrl);
            options.Authenticator = new HttpBasicAuthenticator(username, password);

            return new APIClient(options);
        }

        public APIClient SetRequestTokenAuthentication(string consumerKey, string consumerSecret)
        {
            var options = new RestClientOptions(_baseUrl);
            options.Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);

            return new APIClient(options);
        }

        public APIClient SetAccessTokenAuthentication(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            var options = new RestClientOptions(_baseUrl);
            options.Authenticator = OAuth1Authenticator.ForProtectedResource(consumerKey, consumerSecret, accessToken, accessTokenSecret);

            return new APIClient(options);
        }

        public APIClient SetRequestHeaderAuthentication(string token, string authType = "Bearer")
        {
            var options = new RestClientOptions(_baseUrl);
            options.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, authType);

            return new APIClient(options);
        }

        public APIClient SetJwAuthenticator(string token)
        {
            var options = new RestClientOptions(_baseUrl);
            options.Authenticator = new JwtAuthenticator(token);

            return new APIClient(options);
        }

        public APIClient ClearAuthenticator()
        {
            var options = new RestClientOptions(_baseUrl);

            return new APIClient(options);
        }

        public APIClient AddDefaultHeader(Dictionary<string, string> headers)
        {
            _client.AddDefaultHeaders(headers);
            return this;
        }

        public APIClient CreateRequest(string source = "")
        {
            Request = new RestRequest(source);
            return this;
        }

        public APIClient AddHeader(string name, string value)
        {
            Request.AddHeader(name, value);
            return this;
        }

        public APIClient AddAuthorizationHeader(string value)
        {
            return AddHeader("Authorization", value);
        }

        public APIClient AddContentTypeHeader(string value)
        {
            return AddHeader("Content-Type", value);
        }

        public APIClient AddParameter(string name, string value)
        {
            Request.AddParameter(name, value);
            return this;
        }

        public APIClient AddBody(object obj, string? contentType = null)
        {
            string json = JsonConvert.SerializeObject(obj);
            Request.AddStringBody(json, contentType ?? ContentType.Json);
            return this;
        }

        public async Task<RestResponse> ExecuteGetAsync()
        {
            return await _client.GetAsync(Request);
        }

        public RestResponse ExecuteGet()
        {
            return _client.ExecuteGet(Request);
        }

        public RestResponse<T> ExecuteGet<T>()
        {
            return _client.ExecuteGet<T>(Request);
        }

        public async Task<RestResponse<T>> ExecuteGetAsync<T>()
        {
            return await _client.ExecuteGetAsync<T>(Request);
        }

        public RestResponse ExecutePost()
        {
            return _client.ExecutePost(Request);
        }

        public RestResponse<T> ExecutePost<T>()
        {
            return _client.ExecutePost<T>(Request);
        }

        public async Task<RestResponse> ExecutePostAsync()
        {
            return await _client.ExecutePostAsync(Request);
        }

        public async Task<RestResponse<T>> ExecutePostAsync<T>()
        {
            return await _client.ExecutePostAsync<T>(Request);
        }

        public async Task<RestResponse> ExecuteDeleteAsync()
        {
            Request.Method = Method.Delete;
            return await _client.ExecuteAsync(Request);
        }

        public RestResponse ExecuteDelete()
        {
            Request.Method = Method.Delete;
            return _client.Execute(Request);
        }

        public RestResponse<T> ExecuteDelete<T>()
        {
            Request.Method = Method.Delete;
            return _client.Execute<T>(Request);
        }

        public async Task<RestResponse<T>> ExecuteAsync<T>()
        {
            return await _client.ExecuteAsync<T>(Request);
        }

        public async Task<RestResponse> ExecutePutAsync()
        {
            return await _client.ExecutePutAsync(Request);
        }

        public async Task<RestResponse<T>> ExecutePutAsync<T>()
        {
            return await _client.ExecutePutAsync<T>(Request);
        }

        public RestResponse ExecutePut()
        {
            return _client.ExecutePut(Request);
        }

        public RestResponse<T> ExecutePut<T>()
        {
            return _client.ExecutePut<T>(Request);
        }
    }
}