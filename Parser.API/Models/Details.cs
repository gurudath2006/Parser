using Newtonsoft.Json;

namespace Parser.API.Models
{
    public class Details
    {
        public IEnumerable<string>? Images { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<Tuple<string, int>>? Words { get; set; }
        public bool IsSuccess { get; set; } = true;

    }

    public class Settings
    {
        public string ProxySiteUrl { get; set; }
    }

    public class ApiResponse
    {
        [JsonProperty("contents")]
        public string Contents { get; set; }
        [JsonProperty("status")]
        public ApiStatus Status { get; set; }
    }

    public class ApiStatus
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        [JsonProperty("content_length")]
        public int ContentLength { get; set; }
        [JsonProperty("http_code")]
        public int HttpCode { get; set; }
        [JsonProperty("response_time")]
        public decimal ResponseTime { get; set; }
    }
}
