using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HatsuneMikuMusicShop_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MikuNewsController : ControllerBase
    {
        // 新聞的儲存將採用mongodb資料庫+redis+記憶體快取儲存
        // 新聞一天更新一次，每次更新100筆資料，每頁儲存15筆資料，當使用者下滑時隨機抽取不重複資料更新，更新前將在資料庫的舊資料刪除
        // GET: api/<MikuController>
        [HttpGet]
        public IEnumerable<string> Get()
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
