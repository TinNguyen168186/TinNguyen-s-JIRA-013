using Newtonsoft.Json;

namespace Service.ObjectDto.Response
{
    public class PostBookIsbn
    {
        [JsonProperty("isbn")]
        public string? Isbn { get; set; }
    }

    public class PostBookResponseDto
    {
        [JsonProperty("books")]
        public PostBookIsbn[]? Books { get; set; }
    }
}