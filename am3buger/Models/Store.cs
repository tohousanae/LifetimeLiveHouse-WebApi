using System.ComponentModel.DataAnnotations;

namespace am3burger.Models
{
    public class Store
    {
        [Key]
        [Display(Name = "分店Id")]
        public int Id { get; set; }
    }
}
