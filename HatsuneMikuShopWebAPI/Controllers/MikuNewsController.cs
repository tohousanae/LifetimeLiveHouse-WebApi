using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HatsuneMikuShopWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MikuNewsController : ControllerBase
    {
        // 串接youtube api找vocaloid的最新歌曲
        // 爬到的新聞由python爬蟲爬取，存放在mongoDB中，再由這裡讀取並存到redis中
        // 以上資料皆由redis快取
        // GET: api/<MikuController>
        [HttpGet]
        public IEnumerable<string> GetVocaloidSong()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MikuController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MikuController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MikuController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MikuController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
