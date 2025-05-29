using Newtonsoft.Json;

namespace Service.ObjectDto.Request
{
    public class PostBookRequestDto
    {
        [JsonProperty("userId")]
        public string? UserId { get; set; }

        [JsonProperty("collectionOfIsbns")]
        public IsbnDto[]? CollectionOfIsbns { get; set; }
    }
    
    public class IsbnDto
    {
        [JsonProperty("isbn")]
        public string? Isbn { get; set; }
    }
}