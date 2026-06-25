using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
namespace MyThings.Infrastructure.Services;


public class HybridCacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDatabase _redis;


    public HybridCacheService(IMemoryCache memoryCache, IConnectionMultiplexer redis)
    {
        _memoryCache = memoryCache;
        _redis = redis.GetDatabase();
    }

    public async Task<T?> GetOrCreateAsync<T>(
    string key,
    Func<Task<T?>> factory,
    TimeSpan memoryTtl,
    TimeSpan redisTtl)
    {
        if (_memoryCache.TryGetValue(key, out T? cachedValue))
        {
            Console.WriteLine("Memory Hit");
            return cachedValue;
        }

        var redisValue = await _redis.StringGetAsync(key);

        if (!redisValue.IsNullOrEmpty)
        {
            Console.WriteLine("Redis Hit");

            var deserialized = JsonSerializer.Deserialize<T>(redisValue.ToString());

            _memoryCache.Set(key, deserialized, memoryTtl);

            return deserialized;
        }

        var result = await factory();

        if (result is null)
            return default;

        var json = JsonSerializer.Serialize(result);

        await _redis.StringSetAsync(key, json, redisTtl);

        _memoryCache.Set(key, result, memoryTtl);

        Console.WriteLine("Memory and Redis Miss");

        return result;
    }

    public async Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key);
        await _redis.KeyDeleteAsync(key);
    }

}