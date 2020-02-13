using System.Collections.Generic;
using DrTrendify.Core.Entities;

namespace DrTrendify.Core.Interfaces.Repositories
{
    public interface IStockdetailRepository
    {
        StockDetail GetById(string id);
        IEnumerable<StockDetail> GetAll();
        void Upsert(IEnumerable<StockDetail> stockDetails);
        void Upsert(StockDetail stockDetail);
    }
}