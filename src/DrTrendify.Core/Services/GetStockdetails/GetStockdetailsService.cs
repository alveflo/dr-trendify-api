using System.Collections.Generic;
using DrTrendify.Core.Entities;
using DrTrendify.Core.Interfaces.Repositories;

namespace DrTrendify.Core.Services.GetStockdetails
{
    public class GetStockdetailsService : IGetStockdetailsService
    {
        private readonly IStockdetailRepository _stockdetailRepository;

        public GetStockdetailsService(IStockdetailRepository stockdetailRepository)
        {
            _stockdetailRepository = stockdetailRepository;
        }

        public IEnumerable<StockDetail> GetStockDetails()
        {
            return _stockdetailRepository.GetAll();
        }
    }
}