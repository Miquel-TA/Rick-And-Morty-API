using Newtonsoft.Json;

namespace Domain.Models
{
    public class ExternalApiResponse
    {
        [JsonProperty("info")]
        public Info? Info { get; set; }

        [JsonProperty("results")]
        public List<Character>? Results { get; set; }
    }
}
