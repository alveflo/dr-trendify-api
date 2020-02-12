using System;
using System.Threading.Tasks;
using DrTrendify.Core.Interfaces.Repositories;
using DrTrendify.Core.Entities;
using ServiceStack.Redis;
using System.Collections.Generic;

namespace DrTrendify.Persistance.Redis.Repositories
{
    public class StockdetailRepository : IStockdetailRepository
    {
        private readonly IRedisClient _redisClient;

        public StockdetailRepository(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public IEnumerable<StockDetail> GetAll() 
        {
            var keys = _redisClient.GetAllKeys();
            var result = _redisClient.GetAll<StockDetail>(keys);

            return result.Values;
        }

        public void Upsert(IEnumerable<StockDetail> stockDetails)
        {
            foreach (var stockDetail in stockDetails)
            {
                _redisClient.Add(stockDetail.Id, stockDetail);
            }
        }

        public void Upsert(StockDetail stockDetail)
        {
            _redisClient.Add(stockDetail.Id, stockDetail);
        }
    }
}