using System;
using System.Collections.Generic;
using DrTrendify.Core.Entities;
using DrTrendify.Core.FeatureToggling;
using DrTrendify.Core.Interfaces;
using DrTrendify.Core.Interfaces.Repositories;

namespace DrTrendify.Core.Services.PopulateStockdetails
{
    public class PopulateStockdetailsService : IPopulateStockdetailsService
    {
        private readonly IFeatureFlagProvider _featureFlagProvider;
        private readonly IStocklistFetcher _fetcher;
        private readonly IBabyrageTrendAnalyzer _babyrageTrendAnalyzer;
        private readonly IStockdetailRepository _repository;

        public PopulateStockdetailsService(
            IFeatureFlagProvider featureFlagProvider,
            IStocklistFetcher fetcher,
            IBabyrageTrendAnalyzer babyrageTrendAnalyzer,
            IStockdetailRepository repository)
        {
            _featureFlagProvider = featureFlagProvider;
            _fetcher = fetcher;
            _babyrageTrendAnalyzer = babyrageTrendAnalyzer;
            _repository = repository;
        }

        public void Populate()
        {
            _featureFlagProvider.EnsureFeatureEnabled(FeatureFlags.StocklistFetcherKillSwitch);

            var stockDetails = _fetcher.GetStockDetails();
            stockDetails = AddBabyrageResults(stockDetails);

            _repository.Upsert(stockDetails);
        }

        private IEnumerable<StockDetail> AddBabyrageResults(IEnumerable<StockDetail> stockDetails)
        {
            _featureFlagProvider.EnsureFeatureEnabled(FeatureFlags.BabyrageKillSwitch);

            foreach (var stockDetail in stockDetails)
            {
                if (stockDetail.IsTrending())
                {
                    stockDetail.IsBabyrageTrending = _babyrageTrendAnalyzer.IsTrending(stockDetail);
                }

                stockDetail.LastModifiedDate = DateTime.UtcNow;
            }

            return stockDetails;
        }
    }
}