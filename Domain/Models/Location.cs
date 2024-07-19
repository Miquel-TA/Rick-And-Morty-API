using Newtonsoft.Json;

namespace Domain.Models
{
    public class Location
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }
    }
}
