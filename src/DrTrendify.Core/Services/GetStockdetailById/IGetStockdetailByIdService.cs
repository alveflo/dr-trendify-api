using DrTrendify.Core.Entities;

namespace DrTrendify.Core.Services.GetStockdetailById
{
    public interface IGetStockdetailByIdService
    {
        StockDetail GetById(string id);
    }
}