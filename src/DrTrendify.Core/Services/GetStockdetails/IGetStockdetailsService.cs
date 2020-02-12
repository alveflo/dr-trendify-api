using System.Collections.Generic;
using DrTrendify.Core.Entities;

namespace DrTrendify.Core.Services.GetStockdetails
{
    public interface IGetStockdetailsService
    {
         IEnumerable<StockDetail> GetStockDetails();
    }
}