﻿using System.Linq;
using System.Collections.Generic;
using RestSharp;
using DrTrendify.Core.Interfaces;
using DrTrendify.Core.Entities;
using DrTrendify.NovemberScraper.DataContracts;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace DrTrendify.NovemberScraper
{
    public class NovemberScraperConfig 
    {
        public string BaseUrl { get; set; }
        public string PricePath { get; set; }
        public string HistoryPath { get; set; }
        public string IndicatorPath { get; set; }
        public string AlfaIdListUrl { get; set; }
    }

    public class NovemberStocklistFetcher : IStocklistFetcher
    {
        private readonly RestClient _client;
        private readonly NovemberScraperConfig _config;

        public NovemberStocklistFetcher(IOptions<NovemberScraperConfig> config)
        {
            _config = config.Value;
            _client = new RestClient(_config.BaseUrl);
        }

        public IEnumerable<StockDetail> GetStockDetails()
        {
            var alfaIds = GetAlfaIds(_config.AlfaIdListUrl);

            var stockDetails = Map(
                Get<StockPriceDetail>(_config.PricePath),
                Get<StockHistoryDetail>(_config.HistoryPath),
                Get<StockIndicatorDetail>(_config.IndicatorPath)
            );

            return AddAlfaIds(stockDetails, alfaIds);
        }

        private IEnumerable<StockDetail> AddAlfaIds(IEnumerable<StockDetail> stockDetails, IEnumerable<AlfaIdentifierResponse> alfaIds)
        {
            var noMatches = new List<string>();
            var matches = new List<StockDetail>();

            foreach (var stockDetail in stockDetails)
            {
                var alfaId = alfaIds.FirstOrDefault(x => x.Name.ToLower() == stockDetail.Name.ToLower());

                if (alfaId == null) 
                {
                    if (AlfaIdLookup.Lookup.ContainsKey(stockDetail.Id))
                    {
                        var id = AlfaIdLookup.Lookup[stockDetail.Id];
                        alfaId = alfaIds.First(x => x.Id == id);
                    }
                    else
                    {
                        noMatches.Add(stockDetail.Id);
                    }
                }

                if (alfaId != null)
                {
                    stockDetail.AlfaId = alfaId.Id;
                    matches.Add(stockDetail);
                }
            }

            return matches;
        }

        private T[] Get<T>(string url) where T : class
        {
            var request = new RestRequest(url, DataFormat.Json);
            var response = _client.Get(request);

            return JsonConvert.DeserializeObject<T[]>(response.Content);
        }

        private IEnumerable<AlfaIdentifierResponse> GetAlfaIds(string url)
        {
            var request = new RestRequest(url, DataFormat.Json);
            var response = _client.Get(request);
            
            return JsonConvert.DeserializeObject<AlfaIdentifierResponse[]>(response.Content);
        }

        private IEnumerable<StockDetail> Map(StockPriceDetail[] priceDetails, StockHistoryDetail[] historyDetails, StockIndicatorDetail[] indicatorDetails)
        {
            var stockDetails = new List<StockDetail>();

            var priceDictionary = priceDetails.ToDictionary(x => x.Id);
            var historyDictionary = historyDetails.ToDictionary(x => x.Id);
            var indicatorDictionary = indicatorDetails.ToDictionary(x => x.Id);

            foreach (var priceDetail in priceDetails)
            {
                var id = priceDetail.Id;

                if (!historyDictionary.ContainsKey(id) || !indicatorDictionary.ContainsKey(id))
                {
                    continue;
                }

                stockDetails.Add(MapToStockDetail(priceDictionary[id], historyDictionary[id], indicatorDictionary[id]));
            }

            return stockDetails;
        }

        private StockDetail MapToStockDetail(StockPriceDetail priceDetail, StockHistoryDetail historyDetail, StockIndicatorDetail indicatorDetail)
        {
            return new StockDetail
            {
                Id = priceDetail.Id.ToString(),
                Name = priceDetail.Name,

                DividendYield = indicatorDetail.Dividendyield ?? 0,
                DividendPerShare = indicatorDetail.Dividend ?? 0,

                PricePerEarning = indicatorDetail.PricePerEarnings ?? 0,
                PricePerSales = indicatorDetail.PricePerSales ?? 0,
                EarningsPerShare = indicatorDetail.EarningPerShare ?? 0,

                YieldOneMonth = historyDetail.Diff1Mprc ?? 0,
                YieldThreeMonths = historyDetail.Diff3Mprc ?? 0,
                YieldSixMonths = historyDetail.Diff6Mprc ?? 0,
                YieldOneYear = historyDetail.Diff1Yprc ?? 0
            };
        }
    }
}
