using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HatsuneMIkuShop.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "商品Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "商品名稱")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Display(Name = "商品類別代號")]
        public int TypeId { get; set; }

        [Display(Name = "商品描述")]
        [StringLength(4000)]
        public string Description { get; set; } = null!;

        [Display(Name = "商品圖檔名稱")]
        [StringLength(50)]
        public string PhotoName { get; set; } = null!;

        [Display(Name = "商品價格")]
        public int Price { get; set; }
    }
}
