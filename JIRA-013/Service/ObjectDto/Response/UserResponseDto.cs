using Newtonsoft.Json;

namespace Service.ObjectDto.Response
{
    public class UserResponseDto
    {
        [JsonProperty("userId")]
        public string? UserId { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("books")]
        public BookDto[]? Books { get; set; } = Array.Empty<BookDto>();
    }

    public class BookDto
    {
        [JsonProperty("isbn")]
        public string? Isbn { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("subTitle")]
        public string? SubTitle { get; set; }

        [JsonProperty("author")]
        public string? Author { get; set; }

        [JsonProperty("publish_date")]
        public DateTime? PublishDate { get; set; }

        [JsonProperty("publisher")]
        public string? Publisher { get; set; }

        [JsonProperty("pages")]
        public int? Pages { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("website")]
        public string? Website { get; set; }
    }
}