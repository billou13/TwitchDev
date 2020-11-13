using System.Collections.Generic;

namespace TwitchDev.DataStorage.Interfaces
{
    public interface IRedisService
    {
        T Get<T>(string key);

        void Set<T>(string key, T value);

        void SortedSetAdd<T>(string key, double score, T value);
        IEnumerable<T> SortedSetRangeByScore<T>(string key, double start, double stop);
    }
}
