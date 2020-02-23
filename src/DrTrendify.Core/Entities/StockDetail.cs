using System;

namespace DrTrendify.Core.Entities
{
    public enum Market
    {
        OMX_Stockholm = 1,
        OMX_Copenhagen = 2,
        OMX_Helsinki = 3,
        FirstNorth_Stockholm = 4,
        FirstNorth_Copenhagen = 5,
        Aktietorget = 6
    }

    public class StockDetail
    {
        public string Id { get; set; }
        public Market Market { get; set; }
        public string MarketName => Market.ToString();
        public string AlfaId { get; set; }

        public string Name { get; set; }
        public double Price { get; set; }
        public string MarketplaceCountry { get; set; }
        public string Country { get; set; }

        public double YieldOneMonth { get; set; }
        public double YieldThreeMonths { get; set; }
        public double YieldSixMonths { get; set; }
        public double YieldOneYear { get; set; }

        public double PricePerEarning { get; set; }
        public double PricePerSales { get; set; }
        public double EarningsPerShare { get; set; }
        public double DividendPerShare { get; set; }
        public double DividendYield { get; set; }
        public bool IsBabyrageTrending { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public bool IsAllPositiveYield() => 
                        YieldOneYear > 0
                        && YieldSixMonths > 0
                        && YieldThreeMonths > 0
                        && YieldOneMonth > 0;
        public bool IsTrending() =>
                    IsAllPositiveYield()
                    && YieldOneMonth < YieldThreeMonths
                    && YieldThreeMonths < YieldSixMonths
                    && YieldSixMonths < YieldOneYear;
    }
}