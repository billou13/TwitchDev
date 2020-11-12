using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using TwitchDev.DataStorage.Configuration;
using TwitchDev.DataStorage.Interfaces;

namespace TwitchDev.DataStorage
{
    /// <summary>
    /// To install redis, watch the following video: <see cref="https://youtu.be/YhXeiB_1-uk"/>.
    /// </summary>
    public class RedisService : IRedisService
    {
        private readonly RedisConfiguration _configuration;

        public RedisService(IOptions<RedisConfiguration> options)
        {
            _configuration = options.Value;
        }

        public T Get<T>(string key)
        {
            using (var redis = ConnectionMultiplexer.Connect(_configuration.ConnectionString))
            {
                var database = redis.GetDatabase();
                return JsonConvert.DeserializeObject<T>(database.StringGet(key));
            }
        }

        public void Set<T>(string key, T value)
        {
            using (var redis = ConnectionMultiplexer.Connect(_configuration.ConnectionString))
            {
                var database = redis.GetDatabase();
                database.StringSet(key, JsonConvert.SerializeObject(value));
            }
        }
    }
}
