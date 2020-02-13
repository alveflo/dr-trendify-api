using DrTrendify.Core.Entities;
using DrTrendify.Core.Interfaces.Repositories;

namespace DrTrendify.Core.Services.GetStockdetailById
{
    public class GetStockdetailByIdService : IGetStockdetailByIdService
    {
        private readonly IStockdetailRepository _stockdetailRepository;

        public GetStockdetailByIdService(IStockdetailRepository stockdetailRepository)
        {
            _stockdetailRepository = stockdetailRepository;
        }

        public StockDetail GetById(string id)
        {
            return _stockdetailRepository.GetById(id);
        }
    }
}