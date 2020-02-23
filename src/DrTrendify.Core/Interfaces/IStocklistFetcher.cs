using System.Collections.Generic;
using DrTrendify.Core.Entities;

namespace DrTrendify.Core.Interfaces
{
    public interface IStocklistFetcher
    {
         IEnumerable<StockDetail> GetStockDetails(Market market);
    }
}