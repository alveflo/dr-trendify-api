using System;
using DrTrendify.Core.Entities;
using Newtonsoft.Json;

namespace DrTrendify.NovemberScraper.DataContracts
{
    internal sealed class StockPriceDetail : IStockDetail
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

        [JsonProperty("bidprice")]
        public double? Bidprice { get; set; }

        [JsonProperty("askprice")]
        public double? Askprice { get; set; }

        [JsonProperty("lastprice")]
        public double? Lastprice { get; set; }

        [JsonProperty("dayhighprice")]
        public double? Dayhighprice { get; set; }

        [JsonProperty("daylowprice")]
        public double? Daylowprice { get; set; }

        [JsonProperty("quantity")]
        public long? Quantity { get; set; }

        [JsonProperty("turnover")]
        public double? Turnover { get; set; }

        [JsonProperty("time")]
        public DateTimeOffset? Time { get; set; }
        public Market Market { get; set; }
    }
}