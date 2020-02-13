using DrTrendify.Core.Interfaces;
using DrTrendify.Core.Interfaces.Repositories;

namespace DrTrendify.Core.Services.PopulateStockdetails
{
    public class PopulateStockdetailsService : IPopulateStockdetailsService
    {
        private readonly IStocklistFetcher _fetcher;
        private readonly IStockdetailRepository _repository;

        public PopulateStockdetailsService(
            IStocklistFetcher fetcher,
            IStockdetailRepository repository)
        {
            _fetcher = fetcher;
            _repository = repository;
        }

        public void Populate()
        {
            var stockDetails = _fetcher.GetStockDetails();

            _repository.Upsert(stockDetails);
        }
    }
}