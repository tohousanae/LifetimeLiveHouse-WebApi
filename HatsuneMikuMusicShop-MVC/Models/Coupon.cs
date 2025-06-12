using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HatsuneMikuMusicShop_MVC.Models
{
    public class Coupon
    {
        [Key]
        [Display(Name = "優惠券Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CouponId { get; set; }
    }
}
