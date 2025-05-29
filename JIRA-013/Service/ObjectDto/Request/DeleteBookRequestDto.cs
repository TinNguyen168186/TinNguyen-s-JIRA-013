using Newtonsoft.Json;

namespace Service.ObjectDto.Request
{
    public class DeleteBookRequestDto
    {
        [JsonProperty("isbn")]
        public string? Isbn { get; set; }

        [JsonProperty("userId")]
        public string? UserId { get; set; }
    }
}