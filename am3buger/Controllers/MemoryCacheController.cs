using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace am3burger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemoryCacheController : ControllerBase
    {
        private IMemoryCache _cache { get; set; }
        public MemoryCacheController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        [HttpGet("")]
        public ActionResult<Dictionary<string, string>> Get()
        {
            DateTime cacheEntry;
            // 嘗試取得指定的Cache
            if (!_cache.TryGetValue("CachKey", out cacheEntry))
            {
                // 指定的Cache不存在，所以給予一個新的值
                cacheEntry = DateTime.Now;
                // 設定Cache選項
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // 設定Cache保存時間，如果有存取到就會刷新保存時間
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60));
                // 把資料除存進Cache中
                _cache.Set("CachKey", cacheEntry, cacheEntryOptions);
            }

            return new Dictionary<string, string>()
        {
            {"現在時間", DateTime.Now.ToString()},
            {"快取內容", cacheEntry.ToString()}
        };
        }
    }
}
