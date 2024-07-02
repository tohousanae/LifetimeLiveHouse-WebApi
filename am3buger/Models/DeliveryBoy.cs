using System.ComponentModel.DataAnnotations;

namespace am3burger.Models
{
    public class DeliveryBoy
    {
        [Key]
        [Display(Name = "外送員Id")]
        public int Id { get; set; }
    }
}
