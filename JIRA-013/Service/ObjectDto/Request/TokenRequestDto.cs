using Newtonsoft.Json;

namespace Service.ObjectDto.Request
{
    public class TokenRequestDto
    {
        [JsonProperty("userName")]
        public string? UserName { get; set; }

        [JsonProperty("password")]
        public string? Password { get; set; }
    }
}