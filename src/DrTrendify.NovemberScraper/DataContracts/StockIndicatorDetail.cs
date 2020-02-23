using DrTrendify.Core.Entities;
using Newtonsoft.Json;

namespace DrTrendify.NovemberScraper.DataContracts
{
    internal sealed class StockIndicatorDetail : IStockDetail
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

        [JsonProperty("jek")]
        public double? Jek { get; set; }

        [JsonProperty("solidity")]
        public double? Solidity { get; set; }

        [JsonProperty("dividend")]
        public double? Dividend { get; set; }

        [JsonProperty("dividendyield")]
        public double? Dividendyield { get; set; }

        [JsonProperty("eps")]
        public double? EarningPerShare { get; set; }

        [JsonProperty("per")]
        public double? PricePerEarnings { get; set; }

        [JsonProperty("psr")]
        public double? PricePerSales { get; set; }

        [JsonProperty("profit")]
        public double? Profit { get; set; }

        [JsonProperty("kurs")]
        public double? Kurs { get; set; }

        [JsonProperty("fiscalperiod")]
        public long? Fiscalperiod { get; set; }
        public Market Market { get; set; }
    }
}