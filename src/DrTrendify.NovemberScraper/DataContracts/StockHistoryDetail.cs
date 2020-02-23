using DrTrendify.Core.Entities;
using Newtonsoft.Json;

namespace DrTrendify.NovemberScraper.DataContracts
{
    internal sealed class StockHistoryDetail : IStockDetail
    {
        [JsonProperty("insref")]
        public long Id { get; set; }

        [JsonProperty("marketplace_country")]
        public string MarketplaceCountry { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("diff1d")]
        public double? Diff1D { get; set; }

        [JsonProperty("diff1dprc")]
        public double? Diff1Dprc { get; set; }

        [JsonProperty("diff1wprc")]
        public double? Diff1Wprc { get; set; }

        [JsonProperty("diff1mprc")]
        public double? Diff1Mprc { get; set; }

        [JsonProperty("diff3mprc")]
        public double? Diff3Mprc { get; set; }

        [JsonProperty("diff6mprc")]
        public double? Diff6Mprc { get; set; }

        [JsonProperty("diff1yprc")]
        public double? Diff1Yprc { get; set; }

        [JsonProperty("diffytdprc")]
        public double? Diffytdprc { get; set; }
        public Market Market { get; set; }
  }
}