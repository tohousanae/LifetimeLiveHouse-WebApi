using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouse.Models
{
    public class ProductStatus
    {
        [Key]
        [StringLength(1)]
        [Column(TypeName = "nchar")]
        public string StatusCode { get; set; } = null!; // FK

        [StringLength(10)]
        public string Status { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
