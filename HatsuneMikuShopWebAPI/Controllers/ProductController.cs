
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using HatsuneMikuShopWebAPI.Models;

namespace HatsuneMikuShopWebAPI.Controllers
{
    // 查詢時須限制一次撈出的筆數，避免一次撈出過多資料導致效能問題
    // 快取策略參考：https://www.explainthis.io/zh-hant/swe/cache-mechanism
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redisDb;
        private readonly MikuMusicShopContext _context;
        private readonly IDatabase _cacheDb;

        public ProductController(MikuMusicShopContext context, IConnectionMultiplexer redisService)
        {
            _context = context;
            _redisDb = redisService;
            _cacheDb = _redisDb.GetDatabase();
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Product.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProducts(int id)
        {
            // Read through，優先從快取讀取商品資料，若快取無資料則從資料庫讀取並更新快取
            string cacheKey = $"Product_{id}";

            // 1. 先從 Redis 拿資料
            var cachedProduct = await _cacheDb.StringGetAsync(cacheKey);
            if (cachedProduct.HasValue)
            {
                // Redis 裡存的是字串，需反序列化回 Product 物件
                var product = System.Text.Json.JsonSerializer.Deserialize<Product>(cachedProduct);
                return Ok(product + "來源為快取"); // 修正：使用 Ok() 包裝返回的物件
            }
            else
            {
                // 2. Redis 沒資料，從資料庫拿
                var productFromDb = await _context.Product.FindAsync(id);
                if (productFromDb == null)
                {
                    return NotFound();
                }

                // 3. 寫回 Redis
                var serializedProduct = System.Text.Json.JsonSerializer.Serialize(productFromDb);
                await _cacheDb.StringSetAsync(cacheKey, serializedProduct/*, TimeSpan.FromHours(24) 過期時間*/); 

                return Ok(productFromDb + "來源為資料庫"); // 修正：使用 Ok() 包裝返回的物件
            }
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(int id, Product products)
        {
            if (id != products.Id)
            {
                return BadRequest();
            }

            _context.Entry(products).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProducts(Product products)
        {
            if (ModelState.IsValid == true) //模型驗證是否完全符合規則
            {
                // 用Write-Through Cache，先把商品資料寫入資料庫，資料庫寫入成功後才寫入快取
                _context.Product.Add(products);
                await _context.SaveChangesAsync();

            }
            return CreatedAtAction("GetProducts", new { id = products.Id }, products);

        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            var products = await _context.Product.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            _context.Product.Remove(products);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductsExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
