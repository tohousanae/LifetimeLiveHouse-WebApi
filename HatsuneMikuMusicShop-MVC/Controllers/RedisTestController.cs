using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using am3burger.Models;

namespace HatsuneMikuMusicShop_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisTestController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redisService;

        public RedisTestController(IConnectionMultiplexer redisService)
        {
            _redisService = redisService;
        }

        [HttpGet]
        public ActionResult<string> GetValue(string key)
        {
            var value = _redisService.GetDatabase().StringGet(key).ToString();
            return value;
        }

        [HttpPost]
        public ActionResult<HttpResponse> SetValue(RedisModel redisModel)
        {
            _redisService.GetDatabase().StringSet(redisModel.Key, redisModel.Value);
            return StatusCode(200);

        }
    }
}
