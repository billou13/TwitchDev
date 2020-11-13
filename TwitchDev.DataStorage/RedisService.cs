using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using TwitchDev.DataStorage.Configuration;
using TwitchDev.DataStorage.Interfaces;

namespace TwitchDev.DataStorage
{
    /// <summary>
    /// Redis on Docker installation guide (done on windows 10):
    ///   - Install Docker Desktop from <see cref="https://www.docker.com/get-started"/>
    ///   - Open a command prompt and run the following commands:
    ///       # Download redis image from docker hub
    ///       docker pull redis
    ///       
    ///       # Run redis image exposing port 6379 to 6379 on host machine
    ///       docker run -d -p 6379:6379 --name my-redis redis
    ///       
    ///       # Show docker container to see that status is up
    ///       docker ps -l
    /// 
    /// Source: thank you to <see cref="https://youtu.be/YhXeiB_1-uk"/>.
    /// </summary>
    /// <seealso cref="https://docs.docker.com/engine/reference/commandline/docker"/>
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

        public void SortedSetAdd<T>(string key, double score, T value)
        {
            using (var redis = ConnectionMultiplexer.Connect(_configuration.ConnectionString))
            {
                var database = redis.GetDatabase();
                database.SortedSetAdd(key, JsonConvert.SerializeObject(value), score);
            }
        }

        public IEnumerable<T> SortedSetRangeByScore<T>(string key, double start, double stop)
        {
            using (var redis = ConnectionMultiplexer.Connect(_configuration.ConnectionString))
            {
                var database = redis.GetDatabase();
                var values = database.SortedSetRangeByScore(key, start, stop);
                foreach (var value in values)
                {
                    yield return JsonConvert.DeserializeObject<T>(value);
                }
            }
        }
    }
}
