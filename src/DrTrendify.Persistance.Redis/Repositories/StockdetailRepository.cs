using System.Linq;
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

        public StockDetail GetById(string id)
        {
            return _redisClient.Get<StockDetail>(id);
        }

        public IEnumerable<StockDetail> GetAll() 
        {
            var keys = _redisClient.GetAllKeys();
            var result = _redisClient.GetAll<StockDetail>(keys);

            return result.Values;
        }

        public void Upsert(IEnumerable<StockDetail> stockDetails)
        {
            _redisClient.RemoveAll(stockDetails.Select(x => x.Id));

            foreach (var stockDetail in stockDetails)
                _redisClient.Add(stockDetail.Id, stockDetail);
        }

        public void Upsert(StockDetail stockDetail)
        {
            if (_redisClient.ContainsKey(stockDetail.Id))
                _redisClient.Replace(stockDetail.Id, stockDetail);
            else
                _redisClient.Add(stockDetail.Id, stockDetail);
        }
    }
}