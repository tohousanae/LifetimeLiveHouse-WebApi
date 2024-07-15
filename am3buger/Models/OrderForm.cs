using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace am3burger.Models
{
    public class OrderForm
    {
        [Key]
        [Display(Name = "訂單編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "訂單時間")]
        public DateTime OrderTime { get; set; }

        [Display(Name = "店家名稱")]
        [StringLength(50)]
        public string? Store { get; set; }

        [Display(Name = "商品名稱")]
        [StringLength(50)]
        public string? ProductName { get; set; }

        [Display(Name = "商品圖片名稱")]
        [StringLength(50)]
        public string? PhotoName { get; set; }

        [Display(Name = "商品售價")]
        public int? Price { get; set; }

        [Display(Name = "外送費")]
        public int? Delivery_fee { get; set; }

        [Display(Name = "服務費")]
        public int? Service_charge { get; set; }

        [Display(Name = "總計")]
        public int? Total_price { get; set; }

        [Display(Name = "付款方式")]
        public string? Payment { get; set; }
    }
}
