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

        // stroe表的分店Id
        [Display(Name = "分店Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int store_id { get; set; }
    }
}
