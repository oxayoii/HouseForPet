using DataBaseContext.Dto.RequestModel;
using DataBaseContext.Dto.ResponseModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Redis
{
    public class RedisPets : IRedisPets
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly TimeSpan _cacheExpiration;
        public RedisPets(IConfiguration configuration, IConnectionMultiplexer redis)
        {
            _cacheExpiration = TimeSpan.FromMinutes(configuration.GetValue<int>("CacheSettings:CacheExpirationInMinutes"));
            _redis = redis;
            _database = _redis.GetDatabase();
        }
        public async Task<List<PetsDTO>> GetPetsFromCacheAsync(string cacheKey)
        {
            string cachedPets = await _database.StringGetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedPets))
            {
                return JsonConvert.DeserializeObject<List<PetsDTO>>(cachedPets);
            }
            return null;
        }
        public async Task SetPetsInCacheAsync(string cacheKey, List<PetsDTO> pets)
        {
            string petsJson = JsonConvert.SerializeObject(pets);
            await _database.StringSetAsync(cacheKey, petsJson, _cacheExpiration);
        }
        public async Task InvalidateAllPetsCache()
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: "pets1:*");
            foreach (var key in keys)
            {
                await _database.KeyDeleteAsync(key);
            }
        }
    }
}
