using System.ComponentModel.DataAnnotations;

namespace am3burger.Models
{
    public class OrderForm
    {
        [Key]
        [Display(Name = "訂單Id")]
        public int Id { get; set; }
    }
}
