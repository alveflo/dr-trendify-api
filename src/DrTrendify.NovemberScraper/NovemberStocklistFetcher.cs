using System.Linq;
using System.Collections.Generic;
using RestSharp;
using DrTrendify.Core.Interfaces;
using DrTrendify.Core.Entities;
using DrTrendify.NovemberScraper.DataContracts;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System;

namespace DrTrendify.NovemberScraper
{
    public class NovemberScraperConfig 
    {
        public string BaseUrl { get; set; }
        public string MarketPathsStr { get; set; }
        public string[] MarketPaths => MarketPathsStr.Split(';');
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

        public IEnumerable<StockDetail> GetStockDetails(Market market)
        {
            var alfaIds = GetAlfaIds(_config.AlfaIdListUrl);

            var stockDetails = Map(
                Get<StockPriceDetail>(_config.PricePath, market),
                Get<StockHistoryDetail>(_config.HistoryPath, market),
                Get<StockIndicatorDetail>(_config.IndicatorPath, market)
            );

            return AddAlfaIds(stockDetails, alfaIds, market);
        }

        private IEnumerable<StockDetail> AddAlfaIds(IEnumerable<StockDetail> stockDetails, IEnumerable<AlfaIdentifierResponse> alfaIds, Market market)
        {
            var result = new List<StockDetail>();

            foreach (var stockDetail in stockDetails)
            {
                var normalizedName = stockDetail.Name.ToLower().TrimStart().TrimEnd();
                var alfaId = alfaIds.FirstOrDefault(x => Decode(x.Name) == normalizedName);

                if (alfaId == null) 
                {
                    if (market == Market.OMX_Helsinki)
                    {
                        normalizedName = $"{normalizedName} oyj";
                        alfaId = alfaIds.FirstOrDefault(x => Decode(x.Name) == normalizedName);
                    }
                    else if (AlfaIdLookup.Lookup.ContainsKey(stockDetail.Id))
                    {
                        var id = AlfaIdLookup.Lookup[stockDetail.Id];
                        alfaId = alfaIds.FirstOrDefault(x => x.Id == id);
                    }
                }

                if (alfaId != null)
                {
                    stockDetail.AlfaId = alfaId.Id;
                }

                result.Add(stockDetail);
            }

            return result;
        }

        private T[] Get<T>(string path, Market market) where T : IStockDetail
        {
            var result = new List<T>();

            foreach (var marketPath in _config.MarketPaths)
            {
                var stockMarket = ResolveMarket(marketPath);
                if (stockMarket != market)
                {
                    continue;
                }

                var request = new RestRequest($"{marketPath}/{path}", DataFormat.Json);
                var response = _client.Get(request);

                var deserialized = JsonConvert.DeserializeObject<T[]>(response.Content);
                foreach (var stockDetail in deserialized)
                {
                    stockDetail.Market = stockMarket;
                }

                result.AddRange(deserialized);
            }

            return result.ToArray();
        }

        private Market ResolveMarket(string marketPath)
        {
            switch (marketPath)
            {
                case "stockholm-samtliga":
                    return Market.OMX_Stockholm;
                case "danmark-large":
                case "danmark-mid":
                case "danmark-small":
                    return Market.OMX_Copenhagen;
                case "finland-large":
                case "finland-mid":
                case "finland-small":
                    return Market.OMX_Helsinki;
                case "firstnorth-stockholm":
                    return Market.FirstNorth_Stockholm;
                case "firstnorth-kopenhamn":
                    return Market.FirstNorth_Copenhagen;
                case "aktietorget":
                    return Market.Aktietorget;
                default:
                    throw new ArgumentOutOfRangeException(nameof(marketPath), marketPath, "No such market was available.");
            }
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
                Price = priceDetail.Lastprice ?? 0,
                Market = priceDetail.Market,
                MarketplaceCountry = priceDetail.MarketplaceCountry,
                Country = priceDetail.Country,

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

        private string Decode(string str)
            => str
                .ToLower().TrimStart().TrimEnd()
                .Replace("[x]b4", "´")
                .Replace("[x]c0", "À")   
                .Replace("[x]c1", "Á")   
                .Replace("[x]c2", "Â")   
                .Replace("[x]c3", "Ã")   
                .Replace("[x]c4", "Ä")   
                .Replace("[x]c5", "Å")   
                .Replace("[x]c6", "Æ")   
                .Replace("[x]c7", "Ç")   
                .Replace("[x]c8", "È")   
                .Replace("[x]c9", "É")   
                .Replace("[x]cA", "Ê")   
                .Replace("[x]cB", "Ë")
                .Replace("[x]cc", "Ì")   
                .Replace("[x]cd", "Í")   
                .Replace("[x]ce", "Î")   
                .Replace("[x]cf", "Ï")   
                .Replace("[x]d0", "Ð")
                .Replace("[x]d1", "Ñ")   
                .Replace("[x]d2", "Ò")   
                .Replace("[x]d3", "Ó")   
                .Replace("[x]d4", "Ô")   
                .Replace("[x]d5", "Õ")   
                .Replace("[x]d6", "Ö")   
                .Replace("[x]d7", "×")
                .Replace("[x]d8", "Ø")   
                .Replace("[x]d9", "Ù")   
                .Replace("[x]dA", "Ú")   
                .Replace("[x]dB", "Û")   
                .Replace("[x]dc", "Ü")
                .Replace("[x]dd", "Ý")   
                .Replace("[x]de", "Þ")
                .Replace("[x]df", "ß")
                .Replace("[x]e0", "à")   
                .Replace("[x]e1", "á")
                .Replace("[x]e2", "â")
                .Replace("[x]e3", "ã")
                .Replace("[x]e4", "ä")
                .Replace("[x]e5", "å")
                .Replace("[x]e6", "æ")
                .Replace("[x]e7", "ç")
                .Replace("[x]e8", "è")
                .Replace("[x]e9", "é")   
                .Replace("[x]eA", "ê")   
                .Replace("[x]eB", "ë")   
                .Replace("[x]ec", "ì")
                .Replace("[x]ed", "í")   
                .Replace("[x]ee", "î")   
                .Replace("[x]ef", "ï")   
                .Replace("[x]f0", "ð")   
                .Replace("[x]f1", "ñ")   
                .Replace("[x]f2", "ò")   
                .Replace("[x]f3", "ó")   
                .Replace("[x]f4", "ô")   
                .Replace("[x]f5", "õ")   
                .Replace("[x]f6", "ö")   
                .Replace("[x]f7", "÷")   
                .Replace("[x]f8", "ø")   
                .Replace("[x]f9", "ù")   
                .Replace("[x]fA", "ú")   
                .Replace("[x]fB", "û")   
                .Replace("[x]fc", "ü")   
                .Replace("[x]fd", "ý")   
                .Replace("[x]fe", "þ")   
                .Replace("[x]ff", "ÿ");
    }
}
