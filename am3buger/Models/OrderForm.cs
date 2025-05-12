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

        [Display(Name = "付款方式")]
        public string? Payment { get; set; }
    }
}
