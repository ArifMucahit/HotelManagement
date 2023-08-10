using System.Text.Json;
using HotelManagementAPI.Services.Interface;
using StackExchange.Redis;

namespace HotelManagementAPI.Services;

public class CacheManager : ICacheManager
{
    private readonly IDatabase _db;
    
    public CacheManager(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }
    
    public async Task<T> GetOrAdd<T>(string key, Func<Task<T>> func) where T : class
    {
        var cachedObj = await _db.StringGetAsync(key);
        if (cachedObj.HasValue)
        {
            return JsonSerializer.Deserialize<T>(cachedObj);
        }

        var obj = await func();
        if (obj == null)
        {
            return obj;
        }

        await AddAsync(key, obj);

        return obj;
    }

    public async Task AddAsync(string key, object data)
    {
        string serializedValue = JsonSerializer.Serialize(data);
        await _db.StringSetAsync(key, serializedValue);
    }

    public void Remove(string key)
    {
        _db.KeyDelete(key);
    }
}