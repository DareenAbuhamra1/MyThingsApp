using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CacheController.Controllers
{
    public class CacheController : ControllerBase
    {
        private readonly ILogger<CacheController> _logger;
        private readonly IMemoryCache _cache;
        public CacheController(ILogger<CacheController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet("test-access-speed")]
        public IActionResult TestCachePerformance()
        {
            _cache.Set("TestKey", "Value");

            var sw = Stopwatch.StartNew();

            var val1 = _cache.Get<string>("TestKey");
            sw.Stop();
            var firstAccess = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            var val2 = _cache.Get<string>("TestKey");
            sw.Stop();

            var secondAccess = sw.Elapsed.TotalMilliseconds;
            
            _logger.LogInformation("Access 1: {First}ms, Access 2: {Second}ms", firstAccess, secondAccess);

            return Ok(new { FirstAccess = firstAccess, SecondAccess = secondAccess });
        }
        [HttpGet("test-api-caching")]
        public async Task<IActionResult> SimualateAPICache()
        {
           
            var cacheKey = "ExpData";
            var sw = Stopwatch.StartNew();

            var data = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                await Task.Delay(3000);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return "ExpData";
            });

            sw.Stop();

            var firstAccess = sw.Elapsed.TotalMilliseconds;

            sw.Restart();

            sw.Start();
            var data2 = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                await Task.Delay(3000);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return "ExpData";
            });
            sw.Stop();

            var secondAccess = sw.Elapsed.TotalMilliseconds;

            return Ok(new {CacheMiss = firstAccess, CacheHit = secondAccess });
           
        }
    }
}