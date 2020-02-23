using DrTrendify.Core.Entities;

namespace DrTrendify.Core.Services.PopulateStockdetails
{
    public interface IPopulateStockdetailsService
    {
         void Populate(Market market);
    }
}