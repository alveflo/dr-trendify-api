using Newtonsoft.Json;

namespace DrTrendify.AlfaScraper.DataContracts
{
    internal sealed class AlfaResponse
    {
        [JsonProperty("dataPoints")]
        public double?[][] DataPoints { get; set; }

        [JsonProperty("trendSeries")]
        public double[][] TrendSeries { get; set; }

        [JsonProperty("allowedResolutions")]
        public string[] AllowedResolutions { get; set; }

        [JsonProperty("defaultResolution")]
        public string DefaultResolution { get; set; }

        [JsonProperty("technicalAnalysis")]
        public TechnicalAnalysis[] TechnicalAnalysis { get; set; }

        [JsonProperty("ownersPoints")]
        public object[] OwnersPoints { get; set; }

        [JsonProperty("changePercent")]
        public double ChangePercent { get; set; }

        [JsonProperty("high")]
        public double High { get; set; }

        [JsonProperty("lastPrice")]
        public double LastPrice { get; set; }

        [JsonProperty("low")]
        public double Low { get; set; }
    }

    internal class TechnicalAnalysis
    {
        [JsonProperty("dataPoints")]
        public double[][] DataPoints { get; set; }

        [JsonProperty("timeFrame")]
        public long TimeFrame { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}