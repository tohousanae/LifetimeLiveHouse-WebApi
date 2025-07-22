using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HatsuneMIkuShop.Models
{
    public class Store
    {
        [Key]
        [Display(Name = "分店Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Store_Id { get; set; }
    }
}
