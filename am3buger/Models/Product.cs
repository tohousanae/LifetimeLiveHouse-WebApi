using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace am3burger.Models
{
    [Index(nameof(Name), nameof(Description),nameof(Type))]
    [Table("Product")]
    public class Product
    {
        [Key]
        [Display(Name = "商品Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "商品名稱")]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "商品類別")]
        [StringLength(50)]
        public string? Type { get; set; }

        [Required]
        [Display(Name = "商品描述")]
        [StringLength(4000)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "商品圖檔名稱")]
        [StringLength(50)]
        public string? PhotoName { get; set; }

        [Required]
        [Display(Name = "商品價格")]
        public int? Price { get; set; }
    }
}
