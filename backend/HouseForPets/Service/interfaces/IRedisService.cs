namespace Service.interfaces
{
    public interface IRedisService
    {
        bool IsRequestAllowed(string key, int limit, TimeSpan expiry);
        bool IsUserBlocked(string key);
        void BlockUser(string key, TimeSpan expiry);
    }
}