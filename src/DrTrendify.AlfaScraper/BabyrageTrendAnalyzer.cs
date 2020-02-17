using System;
using System.Linq;
using DrTrendify.Core.Interfaces;
using DrTrendify.Core.Entities;
using DrTrendify.AlfaScraper.DataContracts;
using Microsoft.Extensions.Options;
using RestSharp;
using Newtonsoft.Json;

namespace DrTrendify.AlfaScraper
{
    public class AlfaScraperConfig
    {
        public string BaseUrl { get; set; }
        public string DetailsPath { get; set; }
    }

    internal sealed class AlfaTAValues 
    {
        public double EMA21 { get; set; }
        public double SMA50 { get; set; }
        public double SMA200 { get; set; }
        public double LastPrice { get; set; }
        public double High { get; set; }
    }

    public class BabyrageTrendAnalyzer : IBabyrageTrendAnalyzer
    {
        private readonly AlfaScraperConfig _config;
        private readonly RestClient _client;

        public BabyrageTrendAnalyzer(IOptions<AlfaScraperConfig> config)
        {
            _config = config.Value;
            _client = new RestClient(_config.BaseUrl);
        }

        public bool IsTrending(StockDetail stockDetail)
        {
            if (stockDetail == null)
            {
                throw new ArgumentNullException(nameof(stockDetail));
            }

            return Analyze(Get(stockDetail.AlfaId));
        }

        private bool Analyze(AlfaTAValues values)
        {
            return IsPositiveTrending(values)
                && IsMaTrending(values)
                && IsPriceOk(values)
                && Is52wHigh(values);
        }

        private bool IsPositiveTrending(AlfaTAValues values)
        {
            if (values.EMA21 <= 0)
                return false;
            if (values.SMA50 <= 0)
                return false;
            if (values.SMA200 <= 0)
                return false;

            return true;
        }

        private bool IsMaTrending(AlfaTAValues values)
        {
            if (values.EMA21 <= values.SMA50)
                return false;
            if (values.SMA50 <= values.SMA200)
                return false;

            return true;
        }

        private bool IsPriceOk(AlfaTAValues values)
        {
            if (values.LastPrice <= values.SMA200)
                return false;
            if (values.LastPrice <= values.SMA50)
                return false;
            if (values.LastPrice <= values.EMA21)
                return false;
            
            return true;
        }

        private bool Is52wHigh(AlfaTAValues values)
        {
            if (!(Math.Abs(values.High - values.LastPrice) > 5))
                return false;

            return true;
        }

        private AlfaTAValues Map(AlfaResponse response)
        {
            return new AlfaTAValues
            {
                EMA21 = GetValue(response.TechnicalAnalysis[0]),
                SMA50 = GetValue(response.TechnicalAnalysis[1]),
                SMA200 = GetValue(response.TechnicalAnalysis[2]),
                LastPrice = response.LastPrice,
                High = response.High
            };
        }

        private double GetValue(TechnicalAnalysis dataSet)
        {
            return dataSet.DataPoints[dataSet.DataPoints.Count() - 1][1];
        }

        private AlfaTAValues Get(string stockId)
        {
            var request = (IRestRequest) new RestRequest($"{_config.BaseUrl}/{_config.DetailsPath}", DataFormat.Json);
            var body = new {
                orderbookId = stockId,
                chartType = "area",
                widthOfPlotContainer = 558,
                chartResolution = "day",
                navigator = false,
                percentage = false,
                volume = false,
                owners = false,
                timePeriod = "year",
                ta = new [] {
                    new {
                        type = "ema",
                        timeFrame = 21
                    },
                    new {
                        type = "ma",
                        timeFrame = 50
                    },
                    new {
                        type = "ma",
                        timeFrame = 200
                    },
                },
                compareIds = new object[] {}
            };

            request = request.AddHeader("Content-Type", "application/json");
            request = request.AddHeader("Access-Control-Allow-Origin", "*");
            request = request.AddHeader("Cache-Control", "no-cache");
            request = request.AddHeader("Referer", "http://www.example.com/");
            request = request.AddHeader("Origin", "http://www.example.com/");
            request = request.AddJsonBody(body);

            var response = _client.Post(request);

            var alfaResponse = JsonConvert.DeserializeObject<AlfaResponse>(response.Content);

            return Map(alfaResponse);
        }
    }
}
