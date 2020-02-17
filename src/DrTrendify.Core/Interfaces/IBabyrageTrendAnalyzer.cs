using DrTrendify.Core.Entities;

namespace DrTrendify.Core.Interfaces
{
    public interface IBabyrageTrendAnalyzer
    {
         bool IsTrending(StockDetail stockDetail);
    }
}