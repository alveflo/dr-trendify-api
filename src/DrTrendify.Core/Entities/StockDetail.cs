namespace DrTrendify.Core.Entities
{
    public class StockDetail
    {
        public string Id { get; set; }
        public string AlfaId { get; set; }

        public string Name { get; set; }

        public double YieldOneMonth { get; set; }
        public double YieldThreeMonths { get; set; }
        public double YieldSixMonths { get; set; }
        public double YieldOneYear { get; set; }

        public double PricePerEarning { get; set; }
        public double PricePerSales { get; set; }
        public double EarningsPerShare { get; set; }
        public double DividendPerShare { get; set; }
        public double DividendYield { get; set; }
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