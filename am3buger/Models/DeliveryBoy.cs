using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace am3burger.Models
{
    public class DeliveryBoy
    {
        [Key]
        [Display(Name = "外送員Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        // stroe表分店Id，代表外送員所屬的分店
        [Display(Name = "分店Id")]
        public int store_id { get; set; }
    }
}
