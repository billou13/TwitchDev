namespace TwitchDev.DataStorage.Interfaces
{
    public interface IRedisService
    {
        T Get<T>(string key);

        void Set<T>(string key, T value);
    }
}
