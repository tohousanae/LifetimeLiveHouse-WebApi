using HatsuneMIkuShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HatsuneMikuShopWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        //5.2.4 建立一個新的Post Action，介接口設定為[HttpPost("PostWithPhoto")]，並加入上傳檔案的動作
    //    [HttpPost("PostWithPhoto")]
    //    public async Task<ActionResult<ProductPostDTO>> PostProductWithPhoto([FromForm] ProductPostDTO product)
    //    {
    //        //判斷檔案是否上傳
    //        if (product.Picture == null || product.Picture.Length == 0)
    //        {
    //            return BadRequest("未上傳商品圖片");
    //        }

    //        string fileName = await FileUpload(product.Picture, product.ProductID);

    //        if (fileName == "")
    //        {
    //            return BadRequest("上傳的檔案格式不正確，請上傳jpg、jpeg或png格式的圖片");
    //        }


    //        Product p = new Product
    //        {
    //            ProductID = product.ProductID,
    //            ProductName = product.ProductName,
    //            Price = product.Price,
    //            Description = product.Description,
    //            Picture = fileName,
    //            CateID = product.CateID
    //        };



    //        //寫入資料庫
    //        _context.Product.Add(p);
    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateException)
    //        {
    //            if (ProductExists(product.ProductID))
    //            {
    //                return Conflict();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }


    //        return CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
    //    }
    //}
}
