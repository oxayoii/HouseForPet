using Microsoft.EntityFrameworkCore.Storage;
using Service.interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Redis
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly StackExchange.Redis.IDatabase _database;
        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = _redis.GetDatabase();
        }
        public bool IsRequestAllowed(string key, int limit, TimeSpan expiry)
        {
            var currentCount = _database.StringIncrement(key);
            if (currentCount == 1)
            {
                _database.KeyExpire(key, expiry);
            }
            return currentCount <= limit;
        }
        public bool IsUserBlocked(string key)
        {
            return _database.KeyExists(key);
        }
        public void BlockUser(string key, TimeSpan expiry)
        {
            _database.StringSet(key, "blocked", expiry);
        }

    }
}
