using System.Net;

using Core.Utilities;

using FluentAssertions;

using Newtonsoft.Json;

using NJsonSchema;

using RestSharp;

namespace Core.Extensions
{
    public static class RestExtensions
    {
        public static RestResponse VerifySchema(this RestResponse response, string pathFile)
        {
            var schemaJson = JsonUtils.ReadJsonFile(pathFile);
            var schema = JsonSchema.FromJsonAsync(schemaJson).GetAwaiter().GetResult();

            return response;
        }

        public static dynamic? ConvertToDynamicObject(this RestResponse response)
        {
            if (string.IsNullOrEmpty(response.Content))
            {
                return null;
            }
            return (dynamic?)JsonConvert.DeserializeObject(response.Content);
        }

        public static RestResponse VerifyStatusCodeOk(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            return response;
        }

        public static RestResponse VerifyStatusCodeCreated(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            return response;
        }

        public static RestResponse VerifyStatusCodeBadRequest(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            return response;
        }

        public static RestResponse VerifyStatusCodeUnauthorized(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            return response;
        }

        public static RestResponse VerifyStatusCodeForbidden(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            return response;
        }

        public static RestResponse VerifyStatusCodeInternalServerError(this RestResponse response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            return response;
        }
    }
}