using System.ComponentModel.DataAnnotations;

namespace am3burger.Models
{
    public class Coupon
    {
        [Key]
        [Display(Name = "優惠券Id")]
        public int Id { get; set; }
    }
}
