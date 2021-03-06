using Newtonsoft.Json;

namespace DrTrendify.NovemberScraper.DataContracts
{
    internal sealed class AlfaIdentifierResponse
    {
        [JsonProperty("api_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}