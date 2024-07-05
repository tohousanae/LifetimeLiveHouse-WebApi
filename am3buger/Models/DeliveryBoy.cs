using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace am3burger.Models
{
    [Table("DeliveryBoy")]
    public class DeliveryBoy
    {
        [Key]
        [Display(Name = "外送員Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
