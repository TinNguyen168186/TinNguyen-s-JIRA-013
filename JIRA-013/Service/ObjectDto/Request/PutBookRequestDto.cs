using Newtonsoft.Json;

namespace Service.ObjectDto.Request
{
    public class PutBookRequestDto
    {
        [JsonProperty("userId")]
        public string? UserId { get; set; }

        [JsonProperty("isbn")]
        public string? Isbn { get; set; }
    }
}